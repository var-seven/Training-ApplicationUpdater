using System.Text.Json;
using Common.Extensions;
using UpdateService.Model.Domain;

namespace UpdateService.Manager;

public class FileSystemFileVersionManager : IFileVersionManager
{
    public FileSystemFileVersionManager(IConfiguration configuration)
    {
        _applicationsDirectory =
            new DirectoryInfo(
                configuration.GetConfigValueOrThrow(
                    "ApplicationFileSystemStorage:ApplicationsDirectory"));

        if (!_applicationsDirectory.Exists)
        {
            throw new FileNotFoundException(
                $"The given applications directory {_applicationsDirectory.FullName} does not exists! ");
        }
    }

    private readonly DirectoryInfo _applicationsDirectory;
    private readonly string _jsonFilePattern = "*.json";

    private IEnumerable<FileInfo> GetAllApplicationFiles()
    {
        return _applicationsDirectory.EnumerateFiles(_jsonFilePattern);
    }

    private bool IsApplicationFile(FileInfo file, string applicationName)
    {
        return Path.GetFileNameWithoutExtension(file.Name).Equals(applicationName, StringComparison.OrdinalIgnoreCase);
    }

    public bool ApplicationExists(string applicationName)
    {
        var result = GetAllApplicationFiles().Any(f => IsApplicationFile(f, applicationName));
        return result;
    }

    public Application GetApplication(string applicationName)
    {
        var applicationFile = GetAllApplicationFiles().SingleOrDefault(f => IsApplicationFile(f, applicationName));
        if (applicationFile == null)
        {
            throw new FileNotFoundException($"There is no application file for application {applicationName}");
        }

        var application = JsonSerializer.Deserialize<Application>(
            File.ReadAllBytes(applicationFile.FullName));
        return application ??
               throw new InvalidOperationException(
                   $"Could not deserialize application information from file {applicationFile.FullName}");
    }

    public ApplicationVersion? GetApplicationVersion(string applicationName, Version targetVersion)
    {
        var application = GetApplication(applicationName);
        var applicationVersion = application.Versions.SingleOrDefault(v => v.FileVersion == targetVersion);
        return applicationVersion;
    }

    public ApplicationVersion GetLatestApplicationVersion(string applicationName)
    {
        var application = GetApplication(applicationName);
        var latestVersion = application.Versions.OrderByDescending(v => v.FileVersion).First();

        return latestVersion;
    }
}