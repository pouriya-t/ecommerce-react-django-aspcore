using Application.Orders;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {

        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("add")]
        public async Task<ActionResult<OrderCreatedVM>> Create(Create.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderCreatedVM>> Details(int id)
        {
            return await _mediator.Send(new Detail.Query { Id = id });
        }

        [HttpGet("myorders")]
        public async Task<ActionResult<List<OrderCreatedVM>>> MyOrders()
        {
            return await _mediator.Send(new Application.Orders.List.Query());
        }
    }
}
