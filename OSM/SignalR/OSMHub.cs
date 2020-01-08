using Microsoft.AspNetCore.SignalR;
using OSM.Application.ViewModels.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSM.SignalR
{
    public class OSMHub :Hub
    {
        public async Task SendMessage(AnnouncementViewModel message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
