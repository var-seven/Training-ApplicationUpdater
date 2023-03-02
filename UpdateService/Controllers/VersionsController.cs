using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UpdateService.Manager;
using UpdateService.Model.Transfer;

namespace UpdateService.Controllers;

[ApiController]
[Route("[controller]")]
public class VersionsController : ControllerBase
{
    private readonly IFileVersionManager _fileVersionManager;
    private readonly IMapper _mapper;

    public VersionsController(IFileVersionManager fileVersionManager,
        IMapper mapper)
    {
        _fileVersionManager = fileVersionManager;
        _mapper = mapper;
    }

    [HttpGet("{applicationName}/latest")]
    public IActionResult GetLatestVersion(string applicationName)
    {
        if (!_fileVersionManager.ApplicationExists(applicationName))
        {
            return NotFound();
        }

        var latestVersion = _fileVersionManager.GetLatestApplicationVersion(applicationName);
        return Ok(latestVersion.FileVersion.ToString());
    }
    
    [HttpGet("{applicationName}")]
    public IActionResult GetVersions(string applicationName)
    {
        if (!_fileVersionManager.ApplicationExists(applicationName))
        {
            return NotFound();
        }

        var application = _fileVersionManager.GetApplication(applicationName);
        var result = _mapper.Map<ApplicationDto>(application);
        return Ok(result);
    }
}