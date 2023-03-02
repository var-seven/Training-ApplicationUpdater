using Microsoft.Extensions.Configuration;

namespace Common.Extensions;

public static class ConfigurationExtensions
{
    public static string GetConfigValueOrThrow(this IConfiguration configuration, string configurationKey)
    {
        var value = configuration[configurationKey];
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new KeyNotFoundException($"Missing configuration value for key {configurationKey}");
        }

        return value;
    }
}