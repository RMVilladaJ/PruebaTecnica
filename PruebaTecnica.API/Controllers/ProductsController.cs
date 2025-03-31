using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaTecnica.API.Data;
using PruebaTecnica.Shared.Entities;
using System;
using System.Threading.Tasks;

namespace PruebaTecnica.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] //protegidos
    [ApiController]
    [Route("/api/products")]
    public class ProductsController: ControllerBase
    {
        private readonly DataContext _context;

        public ProductsController(DataContext context)
        {
            _context = context;
        }
        //Get con lista
        //Select * From Products

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts([FromQuery] string search)
        {
            IQueryable<Product> productsQuery = _context.Products;
            if (!string.IsNullOrEmpty(search))
            {
                productsQuery = productsQuery.Where(p => p.NameProduct.Contains(search));
            }

            var products = await productsQuery
                .Include(p => p.Supplier)
                .ToListAsync();

            return Ok(products);
        }

        // Get por parámetro
        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            var owner = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (owner == null)
            {

                return NotFound();
            }

            return Ok(owner);

        }

        //Insertar
        [HttpPost]
        public async Task<ActionResult> Post(Product product)
        {
           _context.Update(product);
            await _context.SaveChangesAsync();
            return Ok(product);
        }


        [HttpPut]
        public async Task<ActionResult> Put(Product product)
        {
            _context.Update(product);
            await _context.SaveChangesAsync();
            return Ok(product);
        }



        //Delete - borrar un registro
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var FilasAfectadas = await _context.Products
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync();

            if (FilasAfectadas == 0)
            {
                return NotFound();
            }
            else
            {
                return NoContent();
            }
        }


    }
}
