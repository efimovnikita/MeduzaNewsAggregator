using Grpc.Core;
using HeadingsLoggerMicroservice.Collector;

namespace HeadingsLoggerMicroservice.Services;

public class LogService : HeadingsLogService.HeadingsLogServiceBase
{
    private readonly ILogCollector _collector;

    public LogService(ILogCollector collector)
    {
        _collector = collector;
    }

    public override Task<Empty> MessageReceived(LogRequest request, ServerCallContext context)
    {
        _collector.HeadingsLogList.Add(request.Title);
        return Task.FromResult(new Empty());
    }
}