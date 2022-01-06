namespace HeadingsLoggerMicroservice.Collector;

public class LogCollector : ILogCollector
{
    public List<string> HeadingsLogList { get; set; } = new();
}