using DesafioCSharpDotNetCore.Data;
using DesafioCSharpDotNetCore.Models;
using DesafioCSharpDotNetCore.Models.InputModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Server.IIS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioCSharpDotNetCore.Controllers
{
    [ApiController]
    [Route("v1/categories")]
    public class CategoryController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context)
        {
            var categories = await context.Categories.ToListAsync();
            return categories;
        }

        [HttpGet]
        [Route("categories/{id:int}")]
        public async Task<ActionResult<Category>> GetById([FromServices] DataContext context, int id)
        {
            var category = await context.Categories
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            return category;
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Category>> Post(
            [FromServices] DataContext context,
            [FromBody] CategoryInputModel model)
        {
            if (ModelState.IsValid)
            {
                Category category = new Category(model.Title);

                context.Categories.Add(category);
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
        public async Task<ActionResult<List<Category>>> Delete([FromServices] DataContext context, int id)
        {
            Category category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)            
                return BadRequest();            

            context.Categories.Remove(category);
            context.SaveChanges();
            var categories = await Get(context);
            return Ok(categories);
        }

        [HttpPut]
        [Route("")]
        public async Task<ActionResult<Category>> Alter(
            [FromServices] DataContext context,
            [FromBody] Category model)            
        {
            var categoryRegistered = GetById(context, model.Id);

            if (categoryRegistered.Result.Value == null || !ModelState.IsValid)
                return BadRequest();

            context.Entry(model).State = EntityState.Modified;
            await context.SaveChangesAsync();
           
            return Ok(model);
        }

    }
}
