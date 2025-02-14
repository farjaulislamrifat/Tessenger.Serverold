using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Models;
using System.Text;
using Tessenger.Server.Data;

namespace Tessenger.Server.Hubs
{
    public class Hub_Methods : Hub
    {
        private readonly IHubContext<Hub_Methods> _hubContext;


        public static Dictionary<string, List<string>> User { get; set; } = new();
        private Dictionary<string, List<string>> GroupUser { get; set; } = new();
        private readonly IDbContextFactory< TessengerServerContext> serverContext;

        public Hub_Methods(IHubContext<Hub_Methods> hubContext, IDbContextFactory<TessengerServerContext> sc)
        {
            serverContext = sc;

            _hubContext = hubContext;
        }

        public async void Add(string username, string password)
        {

            var connectionId = Context.ConnectionId;

            var groups = (await serverContext.CreateDbContextAsync()).Group.ToList();
            var user_ = (await serverContext.CreateDbContextAsync()).User.ToList();
            if (groups != null)
            {
                foreach (var item in groups)
                {
                    if (!GroupUser.Any(c => c.Key == item.Username))
                    {
                        GroupUser.Add(item.Username, new List<string>());
                    }
                }
            }

            var user = user_.FirstOrDefault(c => c.Username == username);
            if (user != null)
            {

            

                if (user.Password == password)
                {
                    if (User.ContainsKey(username))
                    {
                        User[username].Add(connectionId);
                    }
                    else
                    {
                        User.Add(username, new List<string> { connectionId });
                    }
                }
            }
            var meMemberOrAdmin = groups.Where(c => c.Members.Contains(username) || c.Admin == username).ToList();
            if (meMemberOrAdmin != null)
            {
                foreach (var item in meMemberOrAdmin)
                {
                    GroupUser[item.Username].Add(connectionId);
                    await _hubContext.Groups.AddToGroupAsync(connectionId, item.Username);

                }
            }
        }



        public override Task OnConnectedAsync()
        {


            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            try
            {
                var username = User.FirstOrDefault(c => c.Value.Contains(Context.ConnectionId)).Key;
                User[username].Remove(Context.ConnectionId);
                GroupUser[username].Remove(Context.ConnectionId);
                Groups.RemoveFromGroupAsync(Context.ConnectionId, username);
            }
            catch (Exception)
            {

            }
            return base.OnDisconnectedAsync(exception);
        }



        public async Task FriendSendMessage(string username, long id)
        {
            var user = User.FirstOrDefault(c => c.Key == username);
            await Clients.Clients(user.Value).SendAsync("FriendReceiveMessage", username, id);
        }

        public async Task CallRoomSender(string SenderUsername, string RoomId, bool isVideoCall, bool IsGroup , string ReceiverUsername = "")
        {
            if (!IsGroup)
            {
                var user = User.FirstOrDefault(c => c.Key == ReceiverUsername);
                await Clients.Clients(user.Value).SendAsync("CallRoomReceiver", SenderUsername, RoomId , isVideoCall , IsGroup);
            }
            else
            {

            }
        }








    }
}
