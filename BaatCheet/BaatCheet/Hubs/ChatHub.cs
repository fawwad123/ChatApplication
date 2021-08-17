using BaatCheet.Features.ChatFeatures.Commands;
using BaatCheet.Features.ChatFeatures.Queries;
using BusinessLogicLayer.Model;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaatCheet.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMediator mediator;
        private readonly IDictionary<int, string> dictionary;

        public ChatHub(IMediator mediator, IDictionary<int, string> dictionary)
        {
            this.mediator = mediator;
            this.dictionary = dictionary;
        }
        public async Task LoginUser(int userId)
        {
            if(userId != 0 || Context.ConnectionId != null)
                dictionary[userId] = Context.ConnectionId;
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var dic = dictionary.Values;
            foreach(int userId in dictionary.Keys)
            {
                if (dictionary.TryGetValue(userId, out string connectionId))
                    dictionary.Remove(userId);
            }
            return base.OnDisconnectedAsync(exception);
        }
        public async Task SendMessageToFriend(ConversationModel message, int userContactId, int userId)
        {
            try
            {
                var response = await mediator.Send(new GetConversationHashById.Query(userContactId));
                var responseUserId = await mediator.Send(new GetUserId.Query(userContactId, userId));

                if (dictionary.ContainsKey(responseUserId.ContactId))
                    await Groups.AddToGroupAsync(dictionary[responseUserId.ContactId], response.HashCode);

                await Clients.Group(response.HashCode).SendAsync("ReceiveMessage", message);
                await mediator.Send(new SaveConversation.Query(message));
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
                var response = await mediator.Send(new GetGroupHashById.Query(groupId));
                var responseIds = await mediator.Send(new GetAllUsersInGroup.Query(groupId));
                
                foreach (var id in responseIds.UserIds)
                {
                    if (dictionary.TryGetValue(id, out string connectionId))
                        await Groups.AddToGroupAsync(connectionId, response.HashCode);
                }
                await Clients.Group(response.HashCode).SendAsync("ReceiveMessage", message);
                await mediator.Send(new SaveGroupConversation.Query(message));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task JoinFriend(int OldUserContactId, int userContactId)
        {
            try
            {
                var OldResponse = await mediator.Send(new GetConversationHashById.Query(OldUserContactId));
                var response = await mediator.Send(new GetConversationHashById.Query(userContactId));
               /* await Groups.RemoveFromGroupAsync(Context.ConnectionId, OldResponse.HashCode);*/
                await Groups.AddToGroupAsync(Context.ConnectionId, response.HashCode);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task JoinGroup(int OldGroupId, int groupId)
        {
            try
            {
                var oldResponse = await mediator.Send(new GetGroupHashById.Query(OldGroupId));
                var response = await mediator.Send(new GetGroupHashById.Query(groupId));
                /*await Groups.RemoveFromGroupAsync(Context.ConnectionId, oldResponse.HashCode);*/
                await Groups.AddToGroupAsync(Context.ConnectionId, response.HashCode);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task AddNewGroup(string groupName, int createdBy)
        {
            try
            {
                var response = await mediator.Send(new AddNewGroup.Query(groupName, createdBy));
                var responseHash = await mediator.Send(new GetGroupHashById.Query(response.ChatDetails.GroupId));
                await Groups.AddToGroupAsync(dictionary[createdBy], responseHash.HashCode);
                await Clients.Group(responseHash.HashCode).SendAsync("NewGroup", response.ChatDetails, response.Message);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task AddNewContact(string contactEmail, int userId)
        {
            try
            {
                var response = await mediator.Send(new AddNewContact.Query(contactEmail, userId));
                if(response.UserConversation != null)
                {
                    var responseHash = await mediator.Send(new GetConversationHashById.Query(response.UserConversation.UserContactId));
                    if (dictionary.TryGetValue(userId, out string connectionId))
                    {
                        await Groups.AddToGroupAsync(connectionId, responseHash.HashCode);
                    }
                    if (dictionary.TryGetValue(response.UserConversation.Person.Id, out connectionId))
                    {
                        await Groups.AddToGroupAsync(connectionId, responseHash.HashCode);
                    }
                    await Clients.Group(responseHash.HashCode).SendAsync("NewContact", response.UserConversation, response.Message);
                }
                else
                {
                    if (dictionary.TryGetValue(userId, out string connectionId))
                    {
                        await Groups.AddToGroupAsync(connectionId, userId.ToString());
                    }
                    await Clients.Group(userId.ToString()).SendAsync("NewContact", response.UserConversation, response.Message);
                }
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task AddContactToGroup(string contactEmail, int addedBy, string groupId)
        {
            try
            {
                var response = await mediator.Send(new AddUserToGroup.Query(contactEmail, addedBy, int.Parse(groupId)));
                var responseHash = await mediator.Send(new GetGroupHashById.Query(response.ChatDetails.GroupId));
                var groupMember = response.ChatDetails.GroupMember;
                foreach (var member in groupMember)
                {
                    if (dictionary.TryGetValue(member.Id, out string connectionId))
                    {
                        await Groups.AddToGroupAsync(connectionId, responseHash.HashCode);
                    }
                }
                await Clients.Group(responseHash.HashCode).SendAsync("NewContactGroup", response.ChatDetails, response.Message);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
