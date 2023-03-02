using System.Diagnostics;
using Common.Extensions;

namespace Bootstrapper;

public class ExecutableWrapper
{
    public ExecutableWrapper(IConfiguration configuration)
    {
        _executable = new FileInfo(configuration.GetConfigValueOrThrow("ManagedExecutable:FilePath"));
        ApplicationName = configuration.GetConfigValueOrThrow("ManagedExecutable:ApplicationName");
    }

    private readonly FileInfo _executable;

    public string ApplicationName { get; }
    
    public bool Exists => _executable.Exists;

    public string FilePath => _executable.FullName;

    public string? DirectoryPath => _executable.DirectoryName;

    public bool TryGetCurrentVersion(out Version? currentVersion)
    {
        currentVersion = null;
        if (!Exists)
        {
            return false;
        }
        
        var fileVersionInfo = FileVersionInfo.GetVersionInfo(_executable.FullName);
        var fileVersion =
            $"{fileVersionInfo.FileMajorPart}.{fileVersionInfo.FileMinorPart}.{fileVersionInfo.FileBuildPart}";
        return Version.TryParse(fileVersion, out currentVersion);
    }
}