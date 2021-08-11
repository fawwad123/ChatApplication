using BusinessLogicLayer;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BaatCheet.Features.AuthenticationFeatures.Commands
{
    public static class GetUserDetails
    {
        public record Query(string Token) : IRequest<Response>;
        public class GetUserDetailsHandler : IRequestHandler<Query, Response>
        {
            private readonly AuthenticationBLL authenticationBLL;
            public GetUserDetailsHandler()
            {
                this.authenticationBLL = new AuthenticationBLL();
            }
            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                string message = "";
                return new Response(this.authenticationBLL.GetUserDetails(request.Token, ref message));
            }
        }

        public record Response(object UserDetails);
    }
}
