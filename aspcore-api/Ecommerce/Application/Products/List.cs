using Application.Pagination;
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
    public class List
    {
        public class Query : IRequest<ProductPaged>
        {
            public int Page { get; set; }
            public Query(int? page)
            {
                Page = (int)page;
            }
        }

        public class Handler : IRequestHandler<Query, ProductPaged>
        {
            public DataContext Context { get; }
            public Handler(DataContext context)
            {
                Context = context;
            }
            public async Task<ProductPaged> Handle(Query request, CancellationToken cancellationToken)
            {

                // https: //vmsdurano.com/asp-net-core-5-implement-web-api-pagination-with-hateoas-links/
                int limit = 2;

                var queryable = Context.Product.OrderBy(x => x.createdAt).AsQueryable();

                request.Page = (request.Page < 0) ? 1 : request.Page;

                var startRow = (request.Page - 1) * limit;

                var totalPages = (int)Math.Ceiling(queryable.Count() / (double)limit);

                var i = queryable.Count();
                //var totalItemsCountTask = await queryable.CountAsync(cancellationToken);

                var products = await queryable
                        .Skip(startRow)
                        .Take(limit).ToListAsync();

                return new ProductPaged
                {
                    CurrentPage = request.Page,
                    TotalPages = totalPages,
                    TotalItems = i,
                    Products = products
                };

            }
        }
    }
}
