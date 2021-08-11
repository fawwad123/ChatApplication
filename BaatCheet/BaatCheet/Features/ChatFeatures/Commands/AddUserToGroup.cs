using BusinessLogicLayer;
using BusinessLogicLayer.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BaatCheet.Features.ChatFeatures.Commands
{
    public static class AddUserToGroup
    {
        public record Query(string Email, int UserId, int GroupId) : IRequest<Response>;
        public class AddUserToGroupHandler : IRequestHandler<Query, Response>
        {
            private readonly ChatBLL chatBLL;
            public AddUserToGroupHandler()
            {
                this.chatBLL = new ChatBLL();
            }
            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var message = "";
                GroupChatModel chatDetails = this.chatBLL.AddUserToGroup(request.Email, request.UserId, request.GroupId, ref message);
                return (new Response(chatDetails, message));
            }
        }
        public record Response(GroupChatModel ChatDetails, string Message);
    }
}
