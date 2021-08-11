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
    public static class SaveGroupConversation
    {
        public record Query(GroupConversationModel Message) : IRequest<Response>;
        public class GetGroupHashByIdHandler : IRequestHandler<Query, Response>
        {
            private readonly ChatBLL chatBLL;
            public GetGroupHashByIdHandler()
            {
                this.chatBLL = new ChatBLL();
            }
            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                return new Response(chatBLL.SaveGroupConversation(request.Message));
            }
        }
        public record Response(bool Status);
    }
}
