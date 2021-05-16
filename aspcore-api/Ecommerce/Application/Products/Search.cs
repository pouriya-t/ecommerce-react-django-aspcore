using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products
{
    public class Search
    {
        public class Query : IRequest<List<Product>>
        {
            public string SearchName { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<Product>>
        {
            public DataContext Context { get; }
            public Handler(DataContext context)
            {
                Context = context;
            }
            public async Task<List<Product>> Handle(Query request, CancellationToken cancellationToken)
            {
                var products = await Context.Product.Where(o => o.name.Contains(request.SearchName)).ToListAsync();
                return products;
            }
        }
    }
}
