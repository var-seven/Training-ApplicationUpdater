using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UpdateService.Controllers;
using UpdateService.Manager;
using UpdateService.Model.Domain;

namespace UpdateService.Tests.Controller;

public class VersionsControllerTests
{
    [Test]
    public void GetLatestVersion_ApplicationNotExists_Test()
    {
        var fileVersionManagerMock = new Mock<IFileVersionManager>();
        fileVersionManagerMock.Setup(x => x.ApplicationExists(It.IsAny<string>()))
            .Returns(false);
        var mapperMock = new Mock<IMapper>();

        var sut = new VersionsController(fileVersionManagerMock.Object,
            mapperMock.Object);

        var actual = sut.GetLatestVersion("NotepadPlusPlus");
        
        Assert.That(actual, Is.TypeOf<NotFoundResult>());
    }
    
    [Test]
    public void GetLatestVersion_ApplicationExists_Test()
    {
        var fileVersionManagerMock = new Mock<IFileVersionManager>();
        fileVersionManagerMock.Setup(x => x.ApplicationExists(It.IsAny<string>()))
            .Returns(true);
        fileVersionManagerMock.Setup(x => x.GetLatestApplicationVersion(It.IsAny<string>()))
            .Returns(new ApplicationVersion()
            {
                FileVersion = new Version(1, 1, 0),
                ArchiveFilePath = "Foo"
            });
        var mapperMock = new Mock<IMapper>();

        var sut = new VersionsController(fileVersionManagerMock.Object,
            mapperMock.Object);

        var actual = sut.GetLatestVersion("NotepadPlusPlus");
        Assert.That(actual, Is.TypeOf<OkObjectResult>());
        var actualResult = (OkObjectResult)actual;
        Assert.That(actualResult.Value, Is.EqualTo("1.1.0"));
    }
}