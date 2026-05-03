using Microsoft.Extensions.Configuration;

namespace ApiTestingFramework.Core.Configuration;

public sealed class FrameworkConfig : IFrameworkConfig
{
    private const string SectionName = "Framework";

    public FrameworkConfig(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        var section = configuration.GetSection(SectionName);

        BaseUrl = section[nameof(BaseUrl)]
                  ?? throw new InvalidOperationException(
                      $"'{SectionName}:{nameof(BaseUrl)}' is missing in configuration.");

        RequestTimeoutSeconds = section.GetValue(nameof(RequestTimeoutSeconds), 30);
        MinLogLevel = section[nameof(MinLogLevel)] ?? "Info";
    }

    public string BaseUrl { get; }

    public int RequestTimeoutSeconds { get; }

    public string MinLogLevel { get; }

    public static FrameworkConfig LoadDefault()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .Build();

        return new FrameworkConfig(configuration);
    }
}
