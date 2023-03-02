using UpdateService.Manager;
using UpdateService.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IFileVersionManager>(provider =>
{
    var configuration = provider.GetService<IConfiguration>();
    var fileVersionsManagerType = configuration!["FileVersionsManagerType"] ?? "filesystem";
    if (fileVersionsManagerType.ToLowerInvariant() == "filesystem")
    {
        return new FileSystemFileVersionManager(configuration);
    }
    else
    {
        throw new NotSupportedException(
            $"There is no implementation of file versions management for {fileVersionsManagerType}.");
    }
});

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<ApplicationProfile>();
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();