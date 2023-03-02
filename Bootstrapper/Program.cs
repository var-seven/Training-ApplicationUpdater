using Bootstrapper;
using Bootstrapper.Clients;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(builder =>
    {
        builder.AddConsole();
    })
    .ConfigureServices(services =>
    {
        services.AddHttpClient("UpdateServiceClient", (provider, httpClient) =>
            {
                var clientConfig = provider.GetService<ClientConfiguration>();
                httpClient.BaseAddress = clientConfig!.BackendBaseUri;
            })
            .AddTypedClient<FilesClient>()
            .AddTypedClient<VersionsClient>();

        services.AddSingleton<ApplicationInstaller>();
        services.AddSingleton<ExecutableWrapper>();
        services.AddSingleton<ClientConfiguration>();
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();