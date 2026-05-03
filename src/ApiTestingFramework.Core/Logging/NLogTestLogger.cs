using NLog;

namespace ApiTestingFramework.Core.Logging;

public sealed class NLogTestLogger : ITestLogger
{
    private readonly Logger _logger;

    public NLogTestLogger(string name)
    {
        _logger = LogManager.GetLogger(name);
    }

    public void Debug(string message) => _logger.Debug(message);

    public void Info(string message) => _logger.Info(message);

    public void Warn(string message) => _logger.Warn(message);

    public void Error(string message) => _logger.Error(message);

    public void Error(string message, Exception exception) => _logger.Error(exception, message);

    public static void SetMinLevel(string levelName)
    {
        var level = LogLevel.FromString(levelName);
        var config = LogManager.Configuration ?? throw new InvalidOperationException(
            "NLog configuration has not been loaded.");

        foreach (var rule in config.LoggingRules)
        {
            rule.SetLoggingLevels(level, LogLevel.Fatal);
        }

        LogManager.ReconfigExistingLoggers();
    }
}
