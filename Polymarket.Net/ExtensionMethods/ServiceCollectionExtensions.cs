using CryptoExchange.Net;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Interfaces.Clients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using Polymarket.Net;
using Polymarket.Net.Clients;
using Polymarket.Net.Interfaces;
using Polymarket.Net.Interfaces.Clients;
using Polymarket.Net.Objects.Options;
using Polymarket.Net.SymbolOrderBooks;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for DI
    /// </summary>
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// Add services such as the IPolymarketRestClient and IPolymarketSocketClient. Configures the services based on the provided configuration.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configuration">The configuration(section) containing the options</param>
        /// <returns></returns>
        public static IServiceCollection AddPolymarket(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var options = new PolymarketOptions();
            // Reset environment so we know if they're overridden
            options.Rest.Environment = null!;
            options.Socket.Environment = null!;
            configuration.Bind(options);

            if (options.Rest == null || options.Socket == null)
                throw new ArgumentException("Options null");

            var restEnvName = options.Rest.Environment?.Name ?? options.Environment?.Name ?? PolymarketEnvironment.Live.Name;
            var socketEnvName = options.Socket.Environment?.Name ?? options.Environment?.Name ?? PolymarketEnvironment.Live.Name;
            options.Rest.Environment = PolymarketEnvironment.GetEnvironmentByName(restEnvName) ?? options.Rest.Environment!;
            options.Rest.ApiCredentials = options.Rest.ApiCredentials ?? options.ApiCredentials;
            options.Socket.Environment = PolymarketEnvironment.GetEnvironmentByName(socketEnvName) ?? options.Socket.Environment!;
            options.Socket.ApiCredentials = options.Socket.ApiCredentials ?? options.ApiCredentials;


            services.AddSingleton(x => Options.Options.Create(options.Rest));
            services.AddSingleton(x => Options.Options.Create(options.Socket));

            return AddPolymarketCore(services, options.SocketClientLifeTime);
        }

        /// <summary>
        /// Add services such as the IPolymarketRestClient and IPolymarketSocketClient. Services will be configured based on the provided options.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="optionsDelegate">Set options for the Polymarket services</param>
        /// <returns></returns>
        public static IServiceCollection AddPolymarket(
            this IServiceCollection services,
            Action<PolymarketOptions>? optionsDelegate = null)
        {
            var options = new PolymarketOptions();
            // Reset environment so we know if they're overridden
            options.Rest.Environment = null!;
            options.Socket.Environment = null!;
            optionsDelegate?.Invoke(options);
            if (options.Rest == null || options.Socket == null)
                throw new ArgumentException("Options null");

            options.Rest.Environment = options.Rest.Environment ?? options.Environment ?? PolymarketEnvironment.Live;
            options.Rest.ApiCredentials = options.Rest.ApiCredentials ?? options.ApiCredentials;
            options.Socket.Environment = options.Socket.Environment ?? options.Environment ?? PolymarketEnvironment.Live;
            options.Socket.ApiCredentials = options.Socket.ApiCredentials ?? options.ApiCredentials;

            services.AddSingleton(x => Options.Options.Create(options.Rest));
            services.AddSingleton(x => Options.Options.Create(options.Socket));

            return AddPolymarketCore(services, options.SocketClientLifeTime);
        }

        private static IServiceCollection AddPolymarketCore(
            this IServiceCollection services,
            ServiceLifetime? socketClientLifeTime = null)
        {
            services.AddHttpClient<IPolymarketRestClient, PolymarketRestClient>((client, serviceProvider) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<PolymarketRestOptions>>().Value;
                client.Timeout = options.RequestTimeout;
                return new PolymarketRestClient(client, serviceProvider.GetRequiredService<ILoggerFactory>(), serviceProvider.GetRequiredService<IOptions<PolymarketRestOptions>>());
            }).ConfigurePrimaryHttpMessageHandler((serviceProvider) => {
                var options = serviceProvider.GetRequiredService<IOptions<PolymarketRestOptions>>().Value;
                return LibraryHelpers.CreateHttpClientMessageHandler(options.Proxy, options.HttpKeepAliveInterval);
            });
            services.Add(new ServiceDescriptor(typeof(IPolymarketSocketClient), x => { return new PolymarketSocketClient(x.GetRequiredService<IOptions<PolymarketSocketOptions>>(), x.GetRequiredService<ILoggerFactory>()); }, socketClientLifeTime ?? ServiceLifetime.Singleton));

            services.AddTransient<ICryptoRestClient, CryptoRestClient>();
            services.AddSingleton<ICryptoSocketClient, CryptoSocketClient>();
            services.AddTransient<IPolymarketOrderBookFactory, PolymarketOrderBookFactory>();
            services.AddSingleton<IPolymarketUserClientProvider, PolymarketUserClientProvider>(x =>
                new PolymarketUserClientProvider(
                    x.GetRequiredService<HttpClient>(),
                    x.GetRequiredService<ILoggerFactory>(),
                    x.GetRequiredService<IOptions<PolymarketRestOptions>>(),
                    x.GetRequiredService<IOptions<PolymarketSocketOptions>>()));

            return services;
        }
    }
}
