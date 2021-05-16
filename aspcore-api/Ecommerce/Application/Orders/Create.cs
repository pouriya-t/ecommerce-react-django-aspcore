using Application.Users;
using Application.Users.UserAccessor;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
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

namespace Application.Orders
{
    public class Create
    {
        public class Command : IRequest<OrderCreatedVM>
        {
            public OrderItemsVM[] OrderItems { get; set; }

            public ShippingAddress ShippingAddress { get; set; }

            public string PaymentMethod { get; set; }
            public double ItemsPrice { get; set; }
            public double ShippingPrice { get; set; }
            public double TaxPrice { get; set; }
            public double TotalPrice { get; set; }
        }

        public class Handler : IRequestHandler<Command,OrderCreatedVM>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly UserManager<User> _userManager;
            private readonly IMapper _mapper;

            //private readonly DataContext _context;
            public Handler(DataContext context, IUserAccessor userAccessor, UserManager<User> userManager,
                IMapper mapper)
            {
                _context = context;
                _userAccessor = userAccessor;
                _userManager = userManager;
                _mapper = mapper;
            }
            public async Task<OrderCreatedVM> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUsername());

                var order = new Order
                {
                    PaymentMethod = request.PaymentMethod,
                    TaxPrice = request.TaxPrice,
                    ShippingPrice = request.ShippingPrice,
                    TotalPrice = request.TotalPrice,
                    user_id = user.Id,
                };
                _context.Order.Add(order);
                await _context.SaveChangesAsync();

                var objOrder = _context.Order.FirstOrDefault(u => u == order);


                var shippingAddress = new ShippingAddress
                {
                    order_id = objOrder._id,
                    Address = request.ShippingAddress.Address,
                    City = request.ShippingAddress.City,
                    PostalCode = request.ShippingAddress.PostalCode,
                    Country = request.ShippingAddress.Country,
                };
                _context.ShippingAddress.Add(shippingAddress);




                foreach (var item in request.OrderItems)
                {
                    var product = _context.Product.FirstOrDefault(p => p._id == item.Product);

                    var createOrderItems = new OrderItem
                    {
                        product_id = product._id,
                        order_id = objOrder._id,
                        Name = item.Name,
                        Qty = item.Qty,
                        Price = item.Price,
                        Image = item.Image,
                    };

                    _context.OrderItems.Add(createOrderItems);
                }


                var success = await _context.SaveChangesAsync() > 0;



                var createdOrder = _context.OrderItems.Where(o => o.order_id == objOrder._id).ToList();

               

                var createdOrderMapper = _mapper.Map<List<OrderItem>, List<OrderItemDto>>(createdOrder);
                



                if (success)
                {
                    return new OrderCreatedVM
                    {
                        _id = objOrder._id,
                        OrderItems = createdOrderMapper.ToArray(),
                        ShippingAddress = false,
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
                else
                {
                    throw new Exception("some error");
                }

            }
        }
    }
}
