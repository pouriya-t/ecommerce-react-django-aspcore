using Application.Users.UserAccessor;
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

namespace Application.Products
{
    public class Create
    {
        public class Command : IRequest
        {
            //public string name { get; set; }
            //public string image { get; set; }
            //public string brand { get; set; }
            //public string category { get; set; }
            //public string description { get; set; }
            //public double rating { get; set; }
            //public int numReviews { get; set; }
            //public double price { get; set; }
            //public int countInStock { get; set; }
            //public DateTime createdAt { get; set; }

            //public string user_id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly UserManager<User> _userManager;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context,UserManager<User> userManager, IUserAccessor userAccessor)
            {
                _context = context;
                _userManager = userManager;
                _userAccessor = userAccessor;
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                //var product = new Product
                //{
                //    name = request.name,
                //};

                var user = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUsername());

                var product = new Product
                {
                    name = "New Product",
                    image = "/images/default.jpg",
                    brand = "Default Brand",
                    category = "Default Category",
                    description = "Default Description",
                    rating = 0,
                    numReviews = 0,
                    price = 0,
                    countInStock = 0,
                    createdAt = DateTime.Now,
                    user_id = user.Id
                };
                _context.Product.Add(product);
                var success = await _context.SaveChangesAsync() > 0;
                if (success)
                {
                    return Unit.Value;
                }
                else
                {
                    throw new Exception("some error");
                }
            }
        }
    }
}
