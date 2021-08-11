using BusinessLogicLayer;
using BusinessLogicLayer.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BaatCheet.Features.ChatFeatures.Commands
{
    public static class AddNewContact
    {
        public record Query(string Email, int UserId) : IRequest<Response>;
        public class AddNewContactHandler : IRequestHandler<Query, Response>
        {
            private readonly ChatBLL chatBLL;
            public AddNewContactHandler()
            {
                this.chatBLL = new ChatBLL();
            }
            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var message = "";
                UserConversationModel userConversation = this.chatBLL.AddNewContact(request.Email, request.UserId, ref message);
                return new Response(userConversation, message);
            }
        }
        public record Response(UserConversationModel UserConversation, string Message);
    }
}
