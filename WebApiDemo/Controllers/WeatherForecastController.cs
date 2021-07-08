using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using WebApiDemo.Filters;
using WebApiDemo.Hubs;

namespace WebApiDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IHubContext<ChatHub> _chatHub;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IHubContext<ChatHub> chatHub)
        {
            this._chatHub = chatHub;
            _logger = logger;
        }

        [HttpGet]
        [CtmActionFilter]
        public async Task Get(string msg,string cid)
        {
            var hubUser =  HubUser.HubUsers.FirstOrDefault(m=>m.Cid == cid);
            var res = hubUser.UserName +":\r\n"+msg;
            await _chatHub.Clients.All.SendAsync("receiveHello",res);
        }
    }
}
