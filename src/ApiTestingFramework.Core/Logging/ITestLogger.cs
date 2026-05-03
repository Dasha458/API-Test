namespace ApiTestingFramework.Core.Logging;

public interface ITestLogger
{
    void Debug(string message);
    void Info(string message);
    void Warn(string message);
    void Error(string message);
    void Error(string message, Exception exception);
}
