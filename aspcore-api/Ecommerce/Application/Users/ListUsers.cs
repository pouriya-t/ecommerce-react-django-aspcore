using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users
{
    public class ListUsers
    {
        public class Query : IRequest<List<UserDto>> { }

        public class Handler : IRequestHandler<Query, List<UserDto>>
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
            public async Task<List<UserDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = Context.Users.ToList();

                var userList = new List<UserDto>();
                foreach (var item in user)
                {
                    var role = await _userManager.IsInRoleAsync(item, UserRoles.Admin);


                    var userFromMapper = _mapper.Map<User, UserDto>(item);

                    if (role)
                    {
                        userFromMapper.IsAdmin = true;
                    }
                    else
                    {
                        userFromMapper.IsAdmin = false;
                    }

                    userList.Add(new UserDto
                    {
                        _id = userFromMapper._id,
                        Id = userFromMapper.Id,
                        Username = userFromMapper.Username,
                        Name = userFromMapper.Name,
                        Email = userFromMapper.Email,
                        IsAdmin = userFromMapper.IsAdmin
                    });
                }

                



                return userList;
            }
        }
    }
}
