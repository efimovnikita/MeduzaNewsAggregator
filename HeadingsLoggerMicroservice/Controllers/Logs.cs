using Common.Models;
using Common.Services;
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
    public int AddLogItem([FromBody]LogMessageModel? logMessage)
    {
        if (logMessage is null)
        {
            return StatusCodes.Status500InternalServerError;
        }

        _collector.HeadingsLogList.Add(logMessage.Title);
        return StatusCodes.Status201Created;
    }
}