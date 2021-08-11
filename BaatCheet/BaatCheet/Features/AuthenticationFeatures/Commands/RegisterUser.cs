using BusinessLogicLayer;
using BusinessLogicLayer.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BaatCheet.Features.AuthenticationFeatures.Commands
{
    public static class RegisterUser
    {
        public record Query(UserModel user) : IRequest<Response>;
        public class RegisterUserHandler : IRequestHandler<Query, Response>
        {
            private readonly AuthenticationBLL authenticationBLL;
            public RegisterUserHandler()
            {
                this.authenticationBLL = new AuthenticationBLL();
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                return new Response(this.authenticationBLL.RegisterUser(request.user));
            }
        }
        public record Response(string Status);
    }
}
