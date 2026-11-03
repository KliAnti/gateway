using FluentAssertions;
using Grpc.Core;
using Moq;
using System.Net;
using VoicePingRequest = voice.Grpc.PingRequest;
using VoicePongResponse = voice.Grpc.PongResponse;
using PingVoiceResponse = gateway.Features.Voice.Ping.PingVoiceResponse;
using gateway.Tests.Api;
using gateway.Tests.Helpers;

namespace gateway.Tests.Tests
{
    /// <summary>
    /// Проверяет эндпоинт /api/voice/ping (запрос - ответ).
    /// </summary>
    [Collection("Gateway")]
    public class Voice
    {
        private readonly VoiceApi _voiceClient;
        private readonly GatewayFixture _fixture;

        /// <summary>
        /// Получает общую фикстуру и сбрасывает моки.
        /// </summary>
        /// <param name="fixture">Фикстура с поднятым gateway и моками клиентов (микросервисов).</param>
        public Voice(GatewayFixture fixture)
        {
            _fixture = fixture;
            _fixture.Reset();
            _voiceClient = new VoiceApi(fixture.CreateApiClient());
        }

        [Fact]
        public async Task Ping_WhenMessageIsNotProvided_ReturnDefaultMessage()
        {
            // Arrange
            VoicePingRequest? request = null;
            _fixture.VoiceClient
                .Setup(c => c.PingAsync(It.IsAny<VoicePingRequest>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .Callback<VoicePingRequest, Metadata, DateTime?, CancellationToken>((req, _, _, _) => request = req)
                .Returns(GrpcCall.Ok(new VoicePongResponse { Message = "pong" }));

            // Act
            var response = await _voiceClient.Ping();
            var body = await JsonResponseReader.ReadAsAsync<PingVoiceResponse>(response);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            body!.Message.Should().Be("pong");
            request!.Message.Should().Be("Ping");
        }

        [Fact]
        public async Task Ping_WhenMessageIsProvided_ReturnOk()
        {
            // Arrange
            VoicePingRequest? request = null;
            _fixture.VoiceClient
                .Setup(c => c.PingAsync(It.IsAny<VoicePingRequest>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .Callback<VoicePingRequest, Metadata, DateTime?, CancellationToken>((req, _, _, _) => request = req)
                .Returns(GrpcCall.Ok(new VoicePongResponse { Message = "custom pong" }));

            // Act
            var response = await _voiceClient.Ping("hello");
            var body = await JsonResponseReader.ReadAsAsync<PingVoiceResponse>(response);

            // Assert
            body!.Message.Should().Be("custom pong");
            request!.Message.Should().Be("hello");
        }

        [Fact]
        public async Task Ping_WhenIdentityServiceIsUnavailable_ReturnsInternalServerError()
        {
            // Arrange
            _fixture.VoiceClient
                .Setup(c => c.PingAsync(It.IsAny<VoicePingRequest>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
                .Returns(GrpcCall.Error<VoicePongResponse>(StatusCode.Unavailable));

            // Act
            var response = await _voiceClient.Ping();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }
    }
}
