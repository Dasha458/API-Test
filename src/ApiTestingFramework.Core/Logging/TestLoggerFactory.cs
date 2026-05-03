namespace ApiTestingFramework.Core.Logging;

public static class TestLoggerFactory
{
    public static ITestLogger Create<T>() => new NLogTestLogger(typeof(T).FullName ?? typeof(T).Name);

    public static ITestLogger Create(string name) => new NLogTestLogger(name);
}
