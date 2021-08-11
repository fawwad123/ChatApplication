using BusinessLogicLayer;
using BusinessLogicLayer.Model;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BaatCheet.Features.ChatFeatures.Queries
{
    public static class GetGroupHashById
    {
        public record Query(int GroupId) : IRequest<Response>;
        public class GetGroupHashByIdHandler : IRequestHandler<Query, Response>
        {
            private readonly ChatBLL chatBLL;
            public GetGroupHashByIdHandler()
            {
                this.chatBLL = new ChatBLL();
            }
            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                return new Response(chatBLL.GetGroupHash(request.GroupId));
            }
        }
        public record Response(string HashCode);
    }
}
