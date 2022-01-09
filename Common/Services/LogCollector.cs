namespace Common.Services;

public class LogCollector : ILogCollector
{
    public List<string> HeadingsLogList { get; set; } = new();
}