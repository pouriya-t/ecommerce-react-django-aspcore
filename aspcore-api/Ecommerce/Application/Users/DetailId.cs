using AutoMapper;
using Domain;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users
{
    public class DetailId
    {
        public class Query : IRequest<UserDto>
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, UserDto>
        {
            public DataContext Context { get; }

            private readonly IMapper _mapper;
            private readonly UserManager<User> _userManager;

            public Handler(DataContext context,IMapper mapper, UserManager<User> userManager)
            {
                Context = context;
                _mapper = mapper;
                _userManager = userManager;
            }
            public async Task<UserDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await Context.Users.FindAsync(request.Id);

                var userFromMapper = _mapper.Map<User, UserDto>(user);

                var role = await _userManager.IsInRoleAsync(user, UserRoles.Admin);



                if (role)
                {
                    userFromMapper.IsAdmin = true;
                }
                else
                {
                    userFromMapper.IsAdmin = false;
                }

                return userFromMapper;
            }
        }
    }
}
