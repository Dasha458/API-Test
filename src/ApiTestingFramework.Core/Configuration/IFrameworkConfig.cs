namespace ApiTestingFramework.Core.Configuration;

public interface IFrameworkConfig
{
    string BaseUrl { get; }

    int RequestTimeoutSeconds { get; }

    string MinLogLevel { get; }
}
