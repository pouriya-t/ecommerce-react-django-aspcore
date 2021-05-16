using Application.Errors;
using Application.JwtGenerator;
using Domain;
using FluentValidation;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users
{
    public class Register
    {
        public class Command : IRequest<UserVM>
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Username).NotEmpty();
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Password).NotEmpty();
            }
        }
        public class Handler : IRequestHandler<Command, UserVM>
        {
            private readonly DataContext _context;
            private readonly UserManager<User> _userManager;
            private readonly IJwtGenerator _jwtGenerator;

            public Handler(DataContext context, UserManager<User> userManager, IJwtGenerator jwtGenerator)
            {
                _jwtGenerator = jwtGenerator;
                _context = context;
                _userManager = userManager;
            }
            public async Task<UserVM> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await _context.Users.AnyAsync(x => x.Email == request.Email))
                    throw new RestException(HttpStatusCode.BadRequest, new { Email = "Email already exists" });

                if (await _context.Users.AnyAsync(x => x.UserName == request.Username))
                    throw new RestException(HttpStatusCode.BadRequest, new { Email = "Username already exists" });


                var user = new User
                {
                    Email = request.Email,
                    UserName = request.Username
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                var result2 = await _userManager.AddToRoleAsync(user, UserRoles.User);

                if (result.Succeeded && result2.Succeeded)
                {
                    var token = _jwtGenerator.CreateToken(user);
                    return new UserVM
                    {
                        Name = user.UserName,
                        Email = user.Email,
                        Token = token.Result.Token,
                        ValidTo = token.Result.ValidTo
                    };
                }
                throw new Exception("Your request has a problem");
            }
        }
    }
}
