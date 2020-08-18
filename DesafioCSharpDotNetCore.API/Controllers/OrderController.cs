using DesafioCSharpDotNetCore.Data;
using DesafioCSharpDotNetCore.Models;
using DesafioCSharpDotNetCore.Models.InputModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioCSharpDotNetCore.Controllers
{
    [ApiController]
    [Route("v1/orders")]
    public class OrderController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Order>>> Get([FromServices] DataContext context)
        {
            var orders = await context.Orders.Include(x => x.Products).ToListAsync();
            return Ok(orders);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Order>> GetById([FromServices] DataContext context, int id)
        {
            var order = await context.Orders.Include(x => x.Products)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            return Ok(order);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Order>> Post(
            [FromServices] DataContext context,
            [FromBody] OrderInputModel model)
        {
            if (ModelState.IsValid)
            {
                List<Product> products = new List<Product>();

                foreach(int id in model.ProductsId)
                {
                    var productRegistered = await context.Products.Where(x => x.Id == id)
                        .AsNoTracking()
                        .SingleOrDefaultAsync();

                    if (productRegistered == null)
                        return BadRequest();
                    
                    products.Add(productRegistered);
                }

                Order order = new Order(model.Title, model.Description, products);
                
                context.Orders.Add(order);
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
        public async Task<ActionResult<List<Order>>> Delete([FromServices] DataContext context, int id)
        {
            Order order = await context.Orders.Include(x => x.Products)                
                .FirstOrDefaultAsync(x => x.Id == id);

            if (order == null)
                return BadRequest();

            context.Orders.Remove(order);
            context.SaveChanges();
            var orders = await Get(context);

            return Ok(orders);
        }

        [HttpPut]
        [Route("")]
        public async Task<ActionResult<Order>> Alter(
           [FromServices] DataContext context,
           [FromBody] Order model)
        {
            var orderRegistered = GetById(context, model.Id);

            if (orderRegistered.Result.Value == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            context.Entry(model).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return Ok(model);
        }

    }
}
