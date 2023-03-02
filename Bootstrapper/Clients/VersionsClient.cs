namespace Bootstrapper.Clients;

public class VersionsClient
{
    private readonly HttpClient _httpClient;

    public VersionsClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Version> GetLatestVersion(string applicationName, CancellationToken cancellationToken)
    {
        var latestVersionValue = await _httpClient.GetStringAsync(
            $"Versions/{applicationName}/latest", cancellationToken);

        if (!Version.TryParse(latestVersionValue, out var latestVersion))
        {
            throw new ArgumentException($"Version value {latestVersionValue} from service is not valid!");
        }

        return latestVersion;
    }
}