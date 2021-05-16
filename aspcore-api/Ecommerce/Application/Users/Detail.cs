using Application.Users.UserAccessor;
using AutoMapper;
using Domain;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users
{
    public class Detail
    {
        public class Query : IRequest<UserDto>
        {}

        public class Handler : IRequestHandler<Query, UserDto>
        { 

            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;
            private readonly UserManager<User> _userManager;


            public Handler(IMapper mapper, UserManager<User> userManager, IUserAccessor userAccessor)
            {  
                _mapper = mapper;
                _userAccessor = userAccessor;
                _userManager = userManager;
            }
            public async Task<UserDto> Handle(Query request, CancellationToken cancellationToken)
            {

                var user = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUsername());

                var role = await _userManager.IsInRoleAsync(user, UserRoles.Admin);

                
                var userFromMapper = _mapper.Map<User, UserDto>(user);

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
