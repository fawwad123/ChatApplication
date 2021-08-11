using BusinessLogicLayer;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BaatCheet.Features.AuthenticationFeatures.Queries
{
    public static class AuthenticateUser
    {
        public record Query(string Email, string Password) : IRequest<Response>;
        public class AuthenticateUserHandler : IRequestHandler<Query, Response>
        {
            private readonly AuthenticationBLL authenticationBLL;
            public AuthenticateUserHandler()
            {
                this.authenticationBLL = new AuthenticationBLL();
            }
            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                return new Response(authenticationBLL.AuthenticateUser(request.Email, request.Password));
            }
        }
        public record Response(string Token);
    }
}
