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
    public class List
    {
        public class Query : IRequest<List<OrderCreatedVM>> { }

        public class Handler : IRequestHandler<Query, List<OrderCreatedVM>>
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
            public async Task<List<OrderCreatedVM>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUsername());


                var objOrder = _context.Order.Where(u => u.user_id == user.Id).ToList();

                List<OrderCreatedVM> orderCreatedVM = new List<OrderCreatedVM>();

                foreach (var item in objOrder)
                {
                    var shippingAddress = _context.ShippingAddress.FirstOrDefault(s => s.order_id == item._id);
                    var shippingAddressMapper = _mapper.Map<ShippingAddress, ShippingAddressDto>(shippingAddress);


                    var createdOrder = _context.OrderItems.Where(o => o.order_id == item._id).ToList();
                    var createdOrderMapper = _mapper.Map<List<OrderItem>, List<OrderItemDto>>(createdOrder);

                    orderCreatedVM.Add(new OrderCreatedVM
                    {
                        _id = item._id,
                        OrderItems = createdOrderMapper.ToArray(),
                        ShippingAddress = shippingAddressMapper,
                        User = new UserDto
                        {
                            _id = user.Id,
                            Username = user.UserName,
                            Email = user.Email,
                            Name = user.DisplayName,
                        },
                        PaymentMethod = item.PaymentMethod,
                        TaxPrice = item.TaxPrice,
                        ShippingPrice = item.ShippingPrice,
                        TotalPrice = item.TotalPrice,
                        IsPaid = item.IsPaid,
                        PaidAt = item.PaidAt,
                        IsDelivered = item.IsDelivered,
                        DeliveredAt = item.DeliveredAt,
                        CreatedAt = item.CreatedAt,
                    });

                }





                return orderCreatedVM;
            }
        }
    }
}
