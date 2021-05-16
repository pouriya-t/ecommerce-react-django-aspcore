using Application.Products;
using Domain;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {

        // dotnetdetail.net/cqrs-and-mediator-patterns-in-asp-net-core-3-1/
        private readonly IMediator _mediator;
        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<ProductPaged>> List(int? page = 1)
        {

            return await _mediator.Send(new Application.Products.List.Query(page));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Details(int id)
        {
            return await _mediator.Send(new Detail.Query { Id = id });
        }

        [HttpPost("create")]
        public async Task<ActionResult<Unit>> Create(Create.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult<ProductDto>> Edit(int id, Application.Products.Edit.Command command)
        {
            command._id = id;
            return await _mediator.Send(command);
        }

        [HttpPost("upload")]
        public async Task<ActionResult<string>> Upload([FromForm] Upload.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<Product>>> Search(string namelike)
        {

            return await _mediator.Send(new Search.Query { SearchName = namelike });
        }

        //public ProductsController(DataContext context)
        //{
        //    _context = context;
        //}

        //   codewithmukesh.com/blog/cqrs-in-aspnet-core-3-1/
        //   sumantmishra.medium.com/create-a-crud-application-using-asp-net-core-3-0-web-api-a85019cb23c
        //[HttpGet("/api/products")]
        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    return Ok(await Mediator.Send(new List()));
        //}

        //[HttpGet("/api/product/{id}")]
        //public ActionResult<Product> GetProduct(int id)
        //{
        //    var product = _context.Product.FirstOrDefault(p => p._id == id);
        //    return product;
        //}

        //[HttpPut("/api/products/{id}")]
        //public ActionResult<Product> UpdateProduct(int id, Product product)
        //{
        //    var objProduct = _context.Product.FirstOrDefault(p => p._id == id);


        //    if (objProduct._id == id)
        //    {
        //        objProduct.name = product.name;
        //        objProduct.image = product.image;
        //        objProduct.brand = product.brand;
        //        objProduct.category = product.category;
        //        objProduct.description = product.description;
        //        objProduct.rating = product.rating;
        //        objProduct.numReviews = product.numReviews;
        //        objProduct.price = product.price;
        //        objProduct.countInStock = product.countInStock;
        //        objProduct.createdAt = product.createdAt;
        //        objProduct.User = product.User;
        //        _context.Product.Update(objProduct);
        //        _context.SaveChanges();
        //    }
        //    return objProduct;
        //}

        //[HttpPost("/api/products")]
        //[HttpPost]
        //public async Task<IActionResult> AddProduct([FromBody]Product product)
        //{
        //    await _context.Product.AddAsync(product);
        //    await _context.SaveChangesAsync();
        //    return Ok();
        //}
    }
}
