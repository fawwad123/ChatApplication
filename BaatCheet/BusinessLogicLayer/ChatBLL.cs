using AutoMapper;
using BusinessLogicLayer.Model;
using DataAccessLayer;
using DataAccessLayer.Entities;
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
        private readonly Mapper conversationMapper;
        private readonly Mapper groupConversationMapper;

        public ChatBLL()
        {
            chatDAL = new ChatDAL();
            var _configConversation = new MapperConfiguration(configure => configure.CreateMap<Conversation, ConversationModel>().ReverseMap());
            var _configGroupConversation = new MapperConfiguration(configure => configure.CreateMap<GroupConversation, GroupConversationModel>().ReverseMap());
            conversationMapper = new Mapper(_configConversation);
            groupConversationMapper = new Mapper(_configGroupConversation);
        }
        public string GetConversationHash(int userContactId)
        {
            return chatDAL.GetConversationHash(userContactId);
        }

        public void SaveConversation(ConversationModel conversation)
        {
            try
            {
                chatDAL.SaveConversation(conversationMapper.Map<ConversationModel, Conversation>(conversation));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void SaveGroupConversation(GroupConversationModel conversation)
        {
            try
            {
                chatDAL.SaveGroupConversation(groupConversationMapper.Map<GroupConversationModel, GroupConversation>(conversation));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public string GetGroupHash(int groupId)
        {
            return chatDAL.GetGroupHash(groupId);
        }
    }
}
