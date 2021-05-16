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
    public class Delete
    {
        public class Command : IRequest<string>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, string>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                var product = await _context.Product.FindAsync(request.Id);
                if (product == null)
                {
                    throw new Exception("Could not find product");
                }
                _context.Remove(product);
                var success = await _context.SaveChangesAsync() > 0;
                if (success)
                {
                    return "Product Deleted";
                }
                throw new Exception("some problem");
            }
        }
    }
}
