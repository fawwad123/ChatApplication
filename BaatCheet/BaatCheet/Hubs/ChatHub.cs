using BusinessLogicLayer;
using BusinessLogicLayer.Model;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace BaatCheet.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatBLL chatBLL = new ChatBLL();

        public async Task SendMessageToFriend(ConversationModel message, int userContactId)
        {
            try
            {
                await Clients.Group(chatBLL.GetConversationHash(userContactId)).SendAsync("ReceiveMessage", message);
                chatBLL.SaveConversation(message);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        public async Task SendMessageToGroup(GroupConversationModel message, int groupId)
        {
            try
            {
                await Clients.Group(chatBLL.GetGroupHash(groupId)).SendAsync("ReceiveMessage", message);
                chatBLL.SaveGroupConversation(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        public async Task JoinFriend(int OldUserContactId, int userContactId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatBLL.GetConversationHash(OldUserContactId));
            await Groups.AddToGroupAsync(Context.ConnectionId, chatBLL.GetConversationHash(userContactId));
        }
        public async Task JoinGroup(int OldGroupId, int groupId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatBLL.GetGroupHash(OldGroupId));
            await Groups.AddToGroupAsync(Context.ConnectionId, chatBLL.GetGroupHash(groupId));
        }
    }
}
