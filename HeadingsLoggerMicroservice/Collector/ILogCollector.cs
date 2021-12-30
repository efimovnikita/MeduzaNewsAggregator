namespace HeadingsLoggerMicroservice.Collector;

public interface ILogCollector
{
    List<string> HeadingsLogList { get; set; }
}