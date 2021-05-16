using Application.Users;
using Application.Users.UserAccessor;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Orders
{
    public class Detail
    {
        public class Query : IRequest<OrderCreatedVM>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, OrderCreatedVM>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly UserManager<User> _userManager;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IUserAccessor userAccessor, UserManager<User> userManager,
                IMapper mapper)
            {
                _context = context;
                _userAccessor = userAccessor;
                _userManager = userManager;
                _mapper = mapper;
            }
            public async Task<OrderCreatedVM> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUsername());


                var objOrder = _context.Order.FirstOrDefault(u => u._id == request.Id);

                
                if(user.Id != objOrder.user_id)
                    throw new Exception("Your just only access to your orders."); ;


                var shippingAddress = _context.ShippingAddress.FirstOrDefault(s => s.order_id == objOrder._id);
                var shippingAddressMapper = _mapper.Map<ShippingAddress, ShippingAddressDto>(shippingAddress);



                var createdOrder = _context.OrderItems.Where(o => o.order_id == objOrder._id).ToList();
                var createdOrderMapper = _mapper.Map<List<OrderItem>, List<OrderItemDto>>(createdOrder);


                return new OrderCreatedVM
                {
                    _id = objOrder._id,
                    OrderItems = createdOrderMapper.ToArray(),
                    ShippingAddress = shippingAddressMapper,
                    User = new UserDto
                    {
                        _id = user.Id,
                        Username = user.UserName,
                        Email = user.Email,
                        Name = user.DisplayName,
                    },
                    PaymentMethod = objOrder.PaymentMethod,
                    TaxPrice = objOrder.TaxPrice,
                    ShippingPrice = objOrder.ShippingPrice,
                    TotalPrice = objOrder.TotalPrice,
                    IsPaid = objOrder.IsPaid,
                    PaidAt = objOrder.PaidAt,
                    IsDelivered = objOrder.IsDelivered,
                    DeliveredAt = objOrder.DeliveredAt,
                    CreatedAt = objOrder.CreatedAt,
                };
            }
        }
    }
}
