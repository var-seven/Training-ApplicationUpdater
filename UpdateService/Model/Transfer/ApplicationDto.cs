namespace UpdateService.Model.Transfer;

public class ApplicationDto
{
    public string Name { get; set; } = "";

    public string[] Versions { get; set; } = Array.Empty<string>();
}