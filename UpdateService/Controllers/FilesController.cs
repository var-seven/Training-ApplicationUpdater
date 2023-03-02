using Microsoft.AspNetCore.Mvc;
using UpdateService.Manager;

namespace UpdateService.Controllers;

[ApiController]
[Route("[controller]")]
public class FilesController : ControllerBase
{
    private readonly IFileVersionManager _fileVersionManager;

    public FilesController(IFileVersionManager fileVersionManager)
    {
        _fileVersionManager = fileVersionManager;
    }

    [HttpGet("{applicationName}/{targetVersion}")]
    public IActionResult GetSpecificApplicationVersion(string applicationName,
        string targetVersion)
    {
        if (!Version.TryParse(targetVersion, out var appVersion))
        {
            return BadRequest("The given version is invalid");
        }

        if (!_fileVersionManager.ApplicationExists(applicationName))
        {
            return NotFound();
        }

        var applicationVersion = _fileVersionManager.GetApplicationVersion(applicationName, appVersion);
        if (applicationVersion == null)
        {
            return NotFound($"There is no version {targetVersion} available for application {applicationName}");
        }

        return File(
            System.IO.File.OpenRead(applicationVersion.ArchiveFilePath),
            "application/octet-stream",
            Path.GetFileName(applicationVersion.ArchiveFilePath)
        );
    }
    
    [HttpGet("{applicationName}/latest")]
    public IActionResult GetLatestApplicationVersion(string applicationName)
    {
        if (!_fileVersionManager.ApplicationExists(applicationName))
        {
            return NotFound();
        }

        var applicationVersion = _fileVersionManager.GetLatestApplicationVersion(applicationName);

        return File(
            System.IO.File.OpenRead(applicationVersion.ArchiveFilePath),
            "application/octet-stream",
            Path.GetFileName(applicationVersion.ArchiveFilePath)
        );
    }
}