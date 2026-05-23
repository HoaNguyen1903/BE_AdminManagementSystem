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
            await Clients.All.SendAsync("ReceiveAnnouncement", json);
        }

        public async Task BroadcastAnnouncementCreated(object payload)
        {
            string json = JsonConvert.SerializeObject(payload);
            await Clients.All.SendAsync("AnnouncementCreated", json);
        }

        public async Task BroadcastAnnouncementUpdated(object payload)
        {
            string json = JsonConvert.SerializeObject(payload);
            await Clients.All.SendAsync("AnnouncementUpdated", json);
        }

        public async Task BroadcastAnnouncementDeleted(int announcementId)
        {
            await Clients.All.SendAsync("AnnouncementDeleted", announcementId);
        }
    }
}