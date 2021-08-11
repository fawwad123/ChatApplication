using AutoMapper;
using BusinessLogicLayer;
using BusinessLogicLayer.Model;
using DataAccessLayer.Entities;
using DataAccessLayer.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BaatCheet.Features.ChatFeatures.Commands
{
    public static class SaveConversation
    {
        public record Query(ConversationModel Message) : IRequest<Response>;
        public class GetConversationHashByIdHandler : IRequestHandler<Query, Response>
        {
            private readonly ChatBLL chatBLL;
            public GetConversationHashByIdHandler()
            {
                this.chatBLL = new ChatBLL();
            }
            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                return new Response(chatBLL.SaveConversation(request.Message));
            }
        }
        public record Response(bool Status);
    }
}