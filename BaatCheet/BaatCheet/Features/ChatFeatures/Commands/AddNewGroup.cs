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
    public static class AddNewGroup
    {
        public record Query(string GroupName, int CreatedBy) : IRequest<Response>;
        public class AddNewGroupHandler : IRequestHandler<Query, Response>
        {
            private readonly ChatBLL chatBLL;
            public AddNewGroupHandler()
            {
                this.chatBLL = new ChatBLL();
            }
            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    var message = "";
                    GroupChatModel chatDetails = chatBLL.AddNewGroup(request.GroupName, request.CreatedBy, ref message);
                    return new Response(chatDetails, message);
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
        }
        public record Response(GroupChatModel ChatDetails, string Message);
    }
}
