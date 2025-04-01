using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaTecnica.API.Data;
using PruebaTecnica.Shared.Entities;
using PruebaTecnica.Shared.Interfaces;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace PruebaTecnica.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] //protegidos
    [ApiController]
    [Route("/api/products")]
    public class ProductsController: ControllerBase
    {
        private readonly DataContext _context;
        private readonly IS3Service _s3Service;

        public ProductsController(DataContext context, IS3Service s3Service )
        {
            _context = context;
            _s3Service = s3Service;
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

         
            foreach (var product in products)
            {
                if (!string.IsNullOrEmpty(product.Photo))
                {
                    product.Photo = await _s3Service.GetFileUrlAsync(product.Photo);
                }
            }

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
        public async Task<ActionResult> Post([FromForm] IFormFile file, [FromForm] string product)
        {
            if (string.IsNullOrEmpty(product))
            {
                return BadRequest("Product data is missing.");
            }


            Product productObject;
            try
            {
                productObject = JsonSerializer.Deserialize<Product>(product);
            }
            catch (JsonException)
            {
                return BadRequest("Invalid product data.");
            }

            if (file != null && file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var fileKey = "";
                    try
                    {
                        fileKey = await _s3Service.UploadFileAsync(stream, fileName);
                    }
                    catch (Exception ex) {
                        Console.WriteLine(ex.Message);
                    }

                    productObject.Photo = fileKey;
                }
            }

            _context.Update(productObject);
            await _context.SaveChangesAsync();

            return Ok(productObject);
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
