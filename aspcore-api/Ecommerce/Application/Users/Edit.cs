using Application.Errors;
using Application.Users.UserAccessor;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users
{
    public class Edit
    {
        public class Command : IRequest
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }


        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly UserManager<User> _userManager;
            private readonly IPasswordHasher<User> _passwordHasher;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IUserAccessor userAccessor,UserManager<User> userManager
                            , IPasswordHasher<User> passwordHasher)
            {
                _userAccessor = userAccessor;
                _context = context;
                _userManager = userManager;
                _passwordHasher = passwordHasher;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName ==
                                _userAccessor.GetCurrentUsername());

                if (user == null)
                    throw new RestException(HttpStatusCode.NotFound, new { activity = "Not found" });


                if (request.Password != null)
                {
                    user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
                }

                user.DisplayName = request.Name ?? user.DisplayName;
                user.Email = request.Email ?? user.Email;


                var success = await _userManager.UpdateAsync(user);

                if (success.Succeeded)
                {
                    return Unit.Value;
                }
                else
                {

                    throw new Exception("Updated");
                }


            }

        }
    }
}
