namespace UpdateService.Model.Domain;

public class ApplicationVersion
{
    public Version FileVersion { get; set; } = null!;

    public string ArchiveFilePath { get; set; } = "";
}