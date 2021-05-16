using AutoMapper;
using Domain;
using MediatR;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products
{

    public class Edit
    {
        public class Command : IRequest<ProductDto>
        {
            public int _id { get; set; }
            public string name { get; set; }
            public string image { get; set; }
            public string brand { get; set; }
            public string category { get; set; }
            public string description { get; set; }
            public double rating { get; set; }
            public int numReviews { get; set; }
            public double price { get; set; }
            public int countInStock { get; set; }
            public DateTime createdAt { get; set; }
        }

        public class Handler : IRequestHandler<Command,ProductDto>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context,IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<ProductDto> Handle(Command request, CancellationToken cancellationToken)
            {
                
                var product = await _context.Product.FindAsync(request._id);
                if (product == null)
                {
                    throw new Exception("Could not find product");
                }
                product.name = request.name ?? product.name;
                product.brand = request.brand ?? product.brand;
                product.category = request.category ?? product.category;
                product.description = request.description ?? product.description;
                product.rating = request.rating ;
                product.numReviews = request.numReviews;
                product.countInStock = request.countInStock;

                


                



                //product.numReviews = request.numReviews ?? product.numReviews;

                var success = await _context.SaveChangesAsync() > 0;

                var productFromDb = _context.Product.FirstOrDefault(p => p._id == request._id);

                var productMapper = _mapper.Map<Product, ProductDto>(productFromDb);

                if (success)
                {
                    return productMapper;
                }
                throw new Exception("some problem");
            }
        }


    }
}
