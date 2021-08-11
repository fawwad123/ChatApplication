using BusinessLogicLayer;
using BusinessLogicLayer.Model;
using DataAccessLayer.Entities;
using DataAccessLayer.Repository;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BaatCheet.Features.ChatFeatures.Queries
{
    public static class GetConversationHashById
    {
        public record Query(int UserContactId) : IRequest<Response>;
        public class GetConversationHashByIdHandler : IRequestHandler<Query, Response>
        {
            private readonly ChatBLL chatBLL;
            public GetConversationHashByIdHandler()
            {
                this.chatBLL = new ChatBLL();
            }
            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                return new Response(chatBLL.GetConversationHash(request.UserContactId));
            }
        }
        public record Response(string HashCode);
    }   
}
