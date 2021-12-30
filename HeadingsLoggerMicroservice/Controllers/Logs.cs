using Microsoft.AspNetCore.Mvc;

namespace HeadingsLoggerMicroservice.Controllers;

[ApiController]
[Route("[controller]")]
public class Logs
{
    [HttpGet]
    public List<string> Get()
    {
        return new List<string>();
    }
}