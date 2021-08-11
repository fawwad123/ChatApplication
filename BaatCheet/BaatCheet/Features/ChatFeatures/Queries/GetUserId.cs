using BusinessLogicLayer;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BaatCheet.Features.ChatFeatures.Queries
{
    public static class GetUserId
    {
        public record Query(int UserContactId, int UserId) : IRequest<Response>;
        public class GetUserIdHandler : IRequestHandler<Query, Response>
        {
            private readonly ChatBLL chatBLL;
            public GetUserIdHandler()
            {
                this.chatBLL = new ChatBLL();
            }
            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                return new Response(chatBLL.GetUserId(request.UserContactId, request.UserId));
            }
        }
        public record Response(int ContactId);
    }
}
