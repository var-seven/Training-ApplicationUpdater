using UpdateService.Model.Domain;

namespace UpdateService.Manager;

public interface IFileVersionManager
{
    bool ApplicationExists(string applicationName);

    Application GetApplication(string applicationName);

    ApplicationVersion? GetApplicationVersion(string applicationName, Version targetVersion);
    
    ApplicationVersion GetLatestApplicationVersion(string applicationName);
}