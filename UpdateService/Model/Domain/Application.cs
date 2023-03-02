namespace UpdateService.Model.Domain;

public class Application
{
    public string Name { get; set; } = "";

    public ICollection<ApplicationVersion> Versions { get; set; } = new List<ApplicationVersion>();
}