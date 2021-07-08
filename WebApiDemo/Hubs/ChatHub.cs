using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WebApiDemo.Filters;

namespace WebApiDemo.Hubs
{
    public class ChatHub : Hub
    {
        /// <summary>
        /// 连接时调用
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            var cid = Context.ConnectionId;
            Random random = new ();
            var randomNum = random.Next(0, 9);
            if (randomNum > 5)
                await Groups.AddToGroupAsync(cid, "GroupOne");
            else
                await Groups.AddToGroupAsync(cid, "GroupTwo");
            await Clients.Client(cid).SendAsync("receiveHello", cid);
        }
        public async Task SendMsg(string msgText)
        {

            await Clients.Groups("GroupOne").SendAsync("receiveHello", $"{msgText}");
        }
        public async Task SendHello(string name)
        {
           var cid = Context.ConnectionId;
           var hunUser =  new HubUser {
                Cid = cid,
                UserName = name
            };
            HubUser.HubUsers.Add(hunUser);
            await Clients.Client(cid).SendAsync("connectioned", $"{cid}");
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            
            return base.OnDisconnectedAsync(exception);
        }
    }
    public class HubUser{
        public static List<HubUser> HubUsers { get; set; } = new List<HubUser>();
        public string Cid { get; set; }
        public string UserName { get; set; }
    }
}