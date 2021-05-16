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
    public class Detail
    {
        public class Query : IRequest<Product>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Product>
        {
            public DataContext Context { get; }
            public Handler(DataContext context)
            {
                Context = context;
            }
            public async Task<Product> Handle(Query request, CancellationToken cancellationToken)
            {
                var product = await Context.Product.FindAsync(request.Id);
                return product;
            }
        }
    }
}
