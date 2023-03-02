namespace Bootstrapper.Clients;

public class FilesClient
{
    private readonly HttpClient _httpClient;

    public FilesClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<byte[]> GetLatestApplicationVersion(string applicationName, CancellationToken cancellationToken)
    {
        var latestAppVersion = await _httpClient.GetByteArrayAsync(
            $"Files/{applicationName}/latest", cancellationToken);

        return latestAppVersion;
    }
}