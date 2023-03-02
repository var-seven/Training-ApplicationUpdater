using AutoMapper;
using UpdateService.Mapping;

namespace UpdateService.Tests.Mapping;

public class ApplicationProfileTests
{
    [Test]
    public void Profile_Assertion_Test()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ApplicationProfile>();
        });
        configuration.AssertConfigurationIsValid();
    }
}