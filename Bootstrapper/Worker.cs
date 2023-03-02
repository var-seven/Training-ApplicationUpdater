using System.Diagnostics;
using System.Globalization;
using System.IO.Compression;
using Bootstrapper.Clients;

namespace Bootstrapper;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly ExecutableWrapper _executable;
    private readonly VersionsClient _versionsClient;
    private readonly FilesClient _filesClient;
    private readonly ApplicationInstaller _applicationInstaller;

    public Worker(ILogger<Worker> logger,
        IHostApplicationLifetime hostApplicationLifetime,
        ExecutableWrapper executable,
        VersionsClient versionsClient,
        FilesClient filesClient,
        ApplicationInstaller applicationInstaller)
    {
        _logger = logger;
        _hostApplicationLifetime = hostApplicationLifetime;
        _executable = executable;
        _versionsClient = versionsClient;
        _filesClient = filesClient;
        _applicationInstaller = applicationInstaller;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_executable.Exists || !_executable.TryGetCurrentVersion(out var currentExecutableVersion))
        {
            _logger.LogWarning(
                "Target application does not exists or file version could not determined. Install latest version ...");

            await InstallLatestVersion(stoppingToken);
        }
        else
        {
            var latestVersion = await _versionsClient.GetLatestVersion(_executable.ApplicationName, stoppingToken);
            if (latestVersion > currentExecutableVersion)
            {
                _logger.LogInformation("There is a new version {latestVersion} available (current version: {currentVersion}", latestVersion, currentExecutableVersion);

                await InstallLatestVersion(stoppingToken);
            }
            else
            {
                _logger.LogInformation("No new version available.");
            }
        }

        _logger.LogInformation("Start Application");
        Process.Start(_executable.FilePath);

        //await Task.Delay(10000, stoppingToken);
        _hostApplicationLifetime.StopApplication();
    }

    private async Task InstallLatestVersion(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Start downloading latest version..");
        var appArchive = await _filesClient.GetLatestApplicationVersion(_executable.ApplicationName, stoppingToken);
        _logger.LogInformation("Finish downloading latest version..");
        _logger.LogInformation("Start installation");
        
        //Create tmp file
        var zipFilePath = Path.GetTempFileName();
        //Write archive data to tmp file
        await File.WriteAllBytesAsync(zipFilePath, appArchive);
        //Extract archive content to application directory
        ZipFile.ExtractToDirectory(zipFilePath, _executable.DirectoryPath!, true);
        
        _logger.LogInformation("Finish installation");
    }
}