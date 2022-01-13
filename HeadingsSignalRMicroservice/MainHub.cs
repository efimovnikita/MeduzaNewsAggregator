using System.Runtime.CompilerServices;
using System.Timers;
using Common.HelperMethods;
using Common.Models;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Timer = System.Timers.Timer;

namespace HeadingsSignalRMicroservice;

public class MainHub : Hub
{
    private readonly IHttpClientFactory _httpClientFactory;
    private bool _running = true;
    private readonly Timer _timer;

    public MainHub(IHttpClientFactory httpClientFactory, Timer timer)
    {
        _httpClientFactory = httpClientFactory;
        _timer = timer;
    }

    public async IAsyncEnumerable<HeadingModel> GetHeadings([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        _timer.Elapsed += TimeElapsed;
        _timer.Enabled = true;
        
        Console.WriteLine("Start streaming");
        
        while (cancellationToken.IsCancellationRequested == false && _running)
        {
            var httpClient = _httpClientFactory.CreateClient("headings");
            var responseMessage = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get,
                "https://meduza.io/api/v3/search?chrono=news&locale=ru&page=0&per_page=24"), cancellationToken);
            
            if (responseMessage.IsSuccessStatusCode == false) continue;
            
            var contentString = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
            var headingsData = JsonConvert.DeserializeObject<HeadingsDataModel>(contentString);
            var headingModels = Methods.GetHeadingsList("news", headingsData);
            
            await Task.Delay(300000, cancellationToken);
            foreach (var headingModel in headingModels) yield return headingModel;
        }
        
        _timer.Enabled = false;
        Console.WriteLine("Cancel streaming");
        await Clients.Caller.SendAsync("DisconnectSignalR", cancellationToken);
    }

    private void TimeElapsed(object? sender, ElapsedEventArgs e)
    {
        _running = false;
    }
}