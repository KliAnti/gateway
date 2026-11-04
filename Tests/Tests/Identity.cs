using FluentAssertions;
using gateway.Tests.Api;
using gateway.Tests.Helpers;
using Grpc.Core;
using Moq;
using System.Net;
using IdentityPingRequest = identity.Grpc.PingRequest;
using IdentityPongResponse = identity.Grpc.PongResponse;
using PingIdentityResponse = gateway.Features.Identity.Ping.PingIdentityResponse;

namespace gateway.Tests.Tests
{
    /// <summary>
    /// Проверяет эндпоинт /api/identity/ping (запрос - ответ).
    /// </summary>
    [Collection("Gateway")]
    public class Identity
    {
        private readonly IdentityApi _identityClient;
        private readonly GatewayFixture _fixture;

        /// <summary>
        /// Получает общую фикстуру и сбрасывает моки.
        /// </summary>
        /// <param name="fixture">Фикстура с поднятым gateway и моками клиентов (микросервисов).</param>
        public Identity(GatewayFixture fixture)
        {
            _fixture = fixture;
            _fixture.Reset();
            _identityClient = new IdentityApi(fixture.CreateApiClient());
        }

        [Fact]
        public async Task Ping_WhenMessageIsNotProvided_ReturnDefaultMessage()
        {
            // Arrange
            IdentityPingRequest? request = null;
            _fixture.IdentityClient
                .Setup(c => c.PingAsync(It.IsAny<IdentityPingRequest>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .Callback<IdentityPingRequest, Metadata, DateTime?, CancellationToken>((req, _, _, _) => request = req)
                .Returns(GrpcCall.Ok(new IdentityPongResponse { Message = "Pong" }));

            // Act
            var response = await _identityClient.Ping();
            var body = await JsonResponseReader.ReadAsAsync<PingIdentityResponse>(response);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            body!.Message.Should().Be("Pong");
            request!.Message.Should().Be("Ping");
        }

        [Fact]
        public async Task Ping_WhenMessageIsProvided_ReturnOk()
        {
            // Arrange
            IdentityPingRequest? request = null;
            _fixture.IdentityClient
                .Setup(c => c.PingAsync(It.IsAny<IdentityPingRequest>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .Callback<IdentityPingRequest, Metadata, DateTime?, CancellationToken>((req, _, _, _) => request = req)
                .Returns(GrpcCall.Ok(new IdentityPongResponse { Message = "custom pong" }));

            // Act
            var response = await _identityClient.Ping("hello");
            var body = await JsonResponseReader.ReadAsAsync<PingIdentityResponse>(response);

            // Assert
            body!.Message.Should().Be("custom pong");
            request!.Message.Should().Be("hello");
        }

        [Fact]
        public async Task Ping_WhenIdentityServiceIsUnavailable_ReturnsInternalServerError()
        {
            // Arrange
            _fixture.IdentityClient
                .Setup(c => c.PingAsync(It.IsAny<IdentityPingRequest>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .Returns(GrpcCall.Error<IdentityPongResponse>(StatusCode.Unavailable));

            // Act
            var response = await _identityClient.Ping();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }
    }
}