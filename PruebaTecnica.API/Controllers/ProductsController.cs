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
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await _context.Products
                .Include(p => p.Suppliers) // 🔹 Incluir el proveedor relacionado
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
