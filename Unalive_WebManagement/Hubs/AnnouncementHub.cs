using Microsoft.AspNetCore.SignalR;

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
    }
}