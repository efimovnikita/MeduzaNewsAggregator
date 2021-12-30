using HeadingsLoggerMicroservice.Collector;
using Microsoft.AspNetCore.Mvc;

namespace HeadingsLoggerMicroservice.Controllers;

[ApiController]
[Route("[controller]")]
public class Logs
{
    private readonly ILogCollector _collector;
    public Logs(ILogCollector collector)
    {
        _collector = collector;
    }

    [HttpGet]
    public List<string> Get()
    {
        return _collector.HeadingsLogList;
    }

    [HttpPost]
    public void AddLogItem(string item)
    {
        _collector.HeadingsLogList.Add(item);
    }
}