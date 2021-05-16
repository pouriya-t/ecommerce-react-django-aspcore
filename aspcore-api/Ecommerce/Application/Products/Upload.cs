using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products
{
    public class Upload
    {
        public class Command : IRequest<string>
        {
            public int product_id { get; set; }
            public IFormFile Image { get; set; }
        }

        public class Handler : IRequestHandler<Command, string>
        {
            private readonly DataContext _context;
            private readonly IWebHostEnvironment _webHostEnvironment;

            public Handler(DataContext context, IWebHostEnvironment webHostEnvironment)
            {
                _context = context;
                _webHostEnvironment = webHostEnvironment;
            }

            public async Task<string> Handle(Command request, CancellationToken cancellationToken)
            {

                var product = _context.Product.FirstOrDefault(p => p._id == request.product_id);

                string webRootPath = _webHostEnvironment.WebRootPath;

                string upload = webRootPath + WC.ImagePath;
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(request.Image.FileName);

                var replaced = product.image.Replace(@"/images/", "");
                var oldFile = Path.Combine(upload, replaced);


                if (System.IO.File.Exists(oldFile))
                {
                    System.IO.File.Delete(oldFile);
                }


                using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                {
                    request.Image.CopyTo(fileStream);
                }

                //product.image = fileName + extension;
            
                    

                product.image = @"/images/" + fileName + extension;
                   
                
                await _context.SaveChangesAsync();

                

                return "Image Created";


            }


        }
    }
}
