using BusinessLogicLayer;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BaatCheet.Features.ChatFeatures.Queries
{
    public static class GetAllUsersInGroup
    {
        public record Query(int GroupId) : IRequest<Response>;
        public class GetAllUsersInGroupHandler : IRequestHandler<Query, Response>
        {
            private readonly ChatBLL chatBLL;
            public GetAllUsersInGroupHandler()
            {
                this.chatBLL = new ChatBLL();
            }
            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                return new Response(chatBLL.GetAllUsersInGroup(request.GroupId));
            }
        }

        public record Response(List<int> UserIds);
    }
}
