using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReProServices.Infrastructure.HubConfig
{
    public class BroadcastHub : Hub
    {
        public async Task BroadcastData(string data) => await Clients.All.SendAsync("broadcastTest", data);
    }
}
