using DesafioCSharpDotNetCore.Data;
using DesafioCSharpDotNetCore.Models;
using DesafioCSharpDotNetCore.Models.InputModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioCSharpDotNetCore.Controllers
{
    [ApiController]
    [Route("v1/products")]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context)
        {
            var products = await context.Products.Include(x => x.Category).ToListAsync();
            return products;
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Product>> GetById([FromServices] DataContext context, int id)
        {
            Product product = await context.Products.Include(x => x.Category)
                //AsNoTracking used when I wanna just return the data to UI
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            return product;
        }
        
        [HttpGet]
        [Route("categories/{id:int}")]
        public async Task<ActionResult<List<Product>>> GetByCategory([FromServices] DataContext context, int id)
        {
            var products = await context.Products
                .Include(x => x.Category)
                .AsNoTracking()
                .Where(x => x.CategoryId == id)
                //Important! Ever set ToListAsync as the last called method. 
                .ToListAsync();
            return Ok(products);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Product>> Post(
            [FromServices] DataContext context,
            [FromBody] ProductInputModel model)
        {
            if (ModelState.IsValid)
            {
                Product product = new Product(model.Title, model.Description, model.Price, model.CategoryId);

                context.Products.Add(product);
                await context.SaveChangesAsync();

                return Ok(model);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<List<Product>>> Delete([FromServices] DataContext context, int id)
        {
            Product productRegistered = await context.Products.Include(x => x.Category)                         
                .FirstOrDefaultAsync(x => x.Id == id);

            if (productRegistered == null)            
                return BadRequest();
            
            context.Products.Remove(productRegistered);
            context.SaveChanges();
            var products = await Get(context);

            return Ok(products);
        }

        [HttpPut]
        [Route("")]
        public async Task<ActionResult<Product>> Alter(
           [FromServices] DataContext context,
           [FromBody] Product model)
        {
            var productRegistered = GetById(context, model.Id);

            if (productRegistered.Result.Value == null || !ModelState.IsValid)            
                return BadRequest(ModelState);
            
            context.Entry(model).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return Ok(model);
        }
    }
}
