using Common.Extensions;

namespace Bootstrapper.Clients;

public class ClientConfiguration
{
    public ClientConfiguration(IConfiguration configuration)
    {
        BackendBaseUri = new Uri(configuration.GetConfigValueOrThrow("ClientConfiguration:BaseAddress"));
    }

    public Uri BackendBaseUri { get; }
}