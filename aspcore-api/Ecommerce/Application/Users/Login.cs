using Application.Errors;
using Application.JwtGenerator;
using Domain;
using FluentValidation;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
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
    public class Login
    {
        public class Query : IRequest<UserVM>
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.Username).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Query,UserVM>
        {
            private readonly UserManager<User> _userManager;
            private readonly SignInManager<User> _signInManager;
            private readonly IJwtGenerator _jwtGenerator;

            public Handler(UserManager<User> userManager,
                            SignInManager<User> signInManager, IJwtGenerator jwtGenerator)
            {
                _jwtGenerator = jwtGenerator;
                _signInManager = signInManager;
                _userManager = userManager;
            }
            public async Task<UserVM> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(request.Username);

                if (user == null)
                    throw new RestException(HttpStatusCode.Unauthorized);

                var result = await _signInManager.CheckPasswordSignInAsync(user,request.Password, false);

                if (result.Succeeded)
                {
                    var token = _jwtGenerator.CreateToken(user);
                    return new UserVM
                    {
                        _id = user.Id,
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
