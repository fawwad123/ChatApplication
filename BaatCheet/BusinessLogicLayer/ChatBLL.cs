using AutoMapper;
using BusinessLogicLayer.Model;
using DataAccessLayer;
using DataAccessLayer.Entities;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class ChatBLL
    {
        private readonly ChatDAL chatDAL;
        private readonly UserDAL userDAL;
        private readonly Mapper conversationMapper;
        private readonly Mapper groupConversationMapper;
        private readonly Mapper groupChatMapper;
        private readonly Mapper contactMapper;
        private readonly Mapper contactGroupMapper;

        public ChatBLL()
        {
            chatDAL = new ChatDAL();
            userDAL = new UserDAL();
            var _configConversation = new MapperConfiguration(configure => configure.CreateMap<Conversation, ConversationModel>().ReverseMap());
            var _configGroupConversation = new MapperConfiguration(configure => configure.CreateMap<GroupConversation, GroupConversationModel>().ReverseMap());
            var _configGroupChat = new MapperConfiguration(configure => {
                    configure.CreateMap<GroupChat, GroupChatModel>().ReverseMap();
                    configure.CreateMap<Person, PersonModel>().ReverseMap();
                    configure.CreateMap<Chat, ChatModal>().ReverseMap();
            });
            var _configContact = new MapperConfiguration(configure => {
                configure.CreateMap<UserConversation, UserConversationModel>().ReverseMap();
                configure.CreateMap<Person, PersonModel>().ReverseMap();
                configure.CreateMap<Conversation, ConversationModel>().ReverseMap();
            });
            var _configContactGroup = new MapperConfiguration(configure => {
                configure.CreateMap<GroupChat, GroupChatModel>().ReverseMap();
                configure.CreateMap<Person, PersonModel>().ReverseMap();
                configure.CreateMap<Chat, ChatModal>().ReverseMap();
            });
            conversationMapper = new Mapper(_configConversation);
            groupConversationMapper = new Mapper(_configGroupConversation);
            groupChatMapper = new Mapper(_configGroupChat);
            contactMapper = new Mapper(_configContact);
            contactGroupMapper = new Mapper(_configContactGroup);
        }
        public GroupChatModel AddUserToGroup(string email, int userId, int groupId, ref string message)
        {
            try
            {
                return contactGroupMapper.Map<GroupChat, GroupChatModel>(userDAL.AddUserToGroup(email, userId, groupId, ref message));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserConversationModel AddNewContact(string email, int userId, ref string message)
        {
            try
            {
                return contactMapper.Map<UserConversation, UserConversationModel>(userDAL.AddNewContact(email, userId, ref message));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GroupChatModel AddNewGroup(string groupName, int createdBy, ref string message)
        {
            try
            {
                return groupChatMapper.Map<GroupChat, GroupChatModel>(userDAL.AddNewGroup(groupName, createdBy, ref message));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetUserId(int userContactId, int userId)
        {
            return chatDAL.GetUserId(userContactId, userId);
        }

        public List<int> GetAllUsersInGroup(int groupId)
        {
            return chatDAL.GetAllUsersInGroup(groupId);
        }

        public string GetConversationHash(int userContactId)
        {
            return chatDAL.GetConversationHash(userContactId);
        }
        public string GetConversationHashs(int userContactId)
        {
            return chatDAL.GetConversationHash(userContactId);
        }
        public bool SaveConversation(ConversationModel conversation)
        {
            try
            {
                return chatDAL.SaveConversation(conversationMapper.Map<ConversationModel, Conversation>(conversation));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool SaveGroupConversation(GroupConversationModel conversation)
        {
            try
            {
                return chatDAL.SaveGroupConversation(groupConversationMapper.Map<GroupConversationModel, GroupConversation>(conversation));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public string GetGroupHash(int groupId)
        {
            return chatDAL.GetGroupHash(groupId);
        }
    }
}
