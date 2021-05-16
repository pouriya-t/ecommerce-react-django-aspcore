using Application.JwtGenerator;
using Application.Products;
using Application.Users;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {


        // dotnetdetail.net/cqrs-and-mediator-patterns-in-asp-net-core-3-1/
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetUsersList()
        {
            return await _mediator.Send(new ListUsers.Query());
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<ActionResult<UserDto>> CurrentUser()
        {
            return await _mediator.Send(new Application.Users.Detail.Query());
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(string id)
        {
            return await _mediator.Send(new DetailId.Query { Id = id });
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("update/{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(string id,UpdateUser.Command command)
        {
            command._id = id;
            return await _mediator.Send(command);
        }


        [HttpPut("profile/update")]
        public async Task<ActionResult<Unit>> ProfileUpdate(Application.Users.Edit.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserVM>> Register(Register.Command command)
        {
            return await _mediator.Send(command);
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserVM>> Login(Application.Users.Login.Query query)
        {
            return await _mediator.Send(query);
        }



    }
}
