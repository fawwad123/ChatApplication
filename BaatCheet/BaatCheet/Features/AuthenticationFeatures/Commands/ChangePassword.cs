using BusinessLogicLayer;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BaatCheet.Features.AuthenticationFeatures.Commands
{
    public static class ChangePassword
    {
        public record Query(string OldPassword, string NewPassword, string Token) : IRequest<Response>;
        public class ChangePasswordHandler : IRequestHandler<Query, Response>
        {
            private readonly AuthenticationBLL authenticationBLL;
            public ChangePasswordHandler()
            {
                this.authenticationBLL = new AuthenticationBLL();
            }
            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                return new Response(this.authenticationBLL.ChangePassword(request.OldPassword, request.NewPassword, request.Token));
            }
        }
        public record Response(string Status);
    }
}
