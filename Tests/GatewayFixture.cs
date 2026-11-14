using Microsoft.Extensions.DependencyInjection;
using Moq;
using TestFramework.DependencyInjection;
using TestFramework.Fixtures;
using IdentityClient = identity.Grpc.Identity.IdentityClient;
using VoiceClient = voice.Grpc.Voice.VoiceClient;

namespace gateway.Tests
{
    /// <summary>
    /// Поднятие gateway в памяти, без реального порта и сети.
    /// Подмена настоящих микросервисов (клиентов) на их моки.
    /// Фикстура создается единажды на все Gateway тесты.
    /// </summary>
    public class GatewayFixture : MockFixtureBase<Program>
    {
        public Mock<IdentityClient> IdentityClient { get; private set; } = null!;
        public Mock<VoiceClient> VoiceClient { get; private set; } = null!;

        /// <summary>
        /// Подменяет в DI настоящие клиенты на моки перед запуском тестового хоста.
        /// </summary>
        /// <param name="services">Коллекция сервисов тестового хоста.</param>
        protected override void ConfigureMocks(IServiceCollection services)
        {
            IdentityClient = services.RegisterMock<IdentityClient>(MockRegistry);
            VoiceClient = services.RegisterMock<VoiceClient>(MockRegistry);
        }
    }

    /// <summary>
    /// Сообщает xUnit, что все тестовые классы с атрибутом [Collection("Gateway")]
    /// должны получать один общий GatewayFixture.
    /// </summary>
    [CollectionDefinition("Gateway")]
    public class GatewayCollection : ICollectionFixture<GatewayFixture> { }
}