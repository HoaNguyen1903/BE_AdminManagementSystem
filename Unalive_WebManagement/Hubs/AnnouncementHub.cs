using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Unalive_WebManagement.Models;

namespace Unalive_WebManagement.Hubs
{
    public class AnnouncementHub : Hub
    {
        public async Task JoinAnnouncementGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task LeaveAnnouncementGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task SendNewAnnoucement(Announcement announcement)
        {
            string json = JsonConvert.SerializeObject(announcement);
            await Clients.All.SendAsync("ReceiveAnnouncement",json);
        }
    }
}