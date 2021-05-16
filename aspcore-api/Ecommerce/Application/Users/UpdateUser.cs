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
    public class UpdateUser
    {
        public class Command : IRequest<UserDto>
        {
            public string _id { get; set; }

            public string Name { get; set; }

            public string Email { get; set; }

            public bool IsAdmin { get; set; }
        }

        public class Handler : IRequestHandler<Command,UserDto>
        {
            private readonly DataContext _context;
            private readonly UserManager<User> _userManager;
            private readonly IMapper _mapper;

            public Handler(DataContext context, UserManager<User> userManager, IMapper mapper)
            {
                _context = context;
                _userManager = userManager;
                _mapper = mapper;
            }

            public async Task<UserDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FindAsync(request._id);
                if (user == null)
                {
                    throw new Exception("Could not find book");
                }
                user.DisplayName = request.Name ?? user.DisplayName;
                user.Email = request.Email ?? user.Email;
                user.DisplayName = request.Name ?? user.DisplayName;
                
                if(request.IsAdmin == true)
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.Admin);
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.User);
                }
                
                
                
                await _context.SaveChangesAsync();


                var userUpdated = _context.Users.FirstOrDefault(u => u.Id == user.Id);

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
                
                throw new Exception("some problem");
            }
        }
    }
}
