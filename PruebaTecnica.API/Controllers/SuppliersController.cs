using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaTecnica.API.Data;
using PruebaTecnica.Shared.Entities;
using System.Threading.Tasks;

namespace PruebaTecnica.API.Controllers
{
    [ApiController]
    [Route("/api/suppliers")]


    public class SuppliersController:ControllerBase
    {
        private readonly DataContext _context;

        public SuppliersController(DataContext context)
        {
            _context = context;
        }
        //Get con lista
        //Select * From suppliers

        [HttpGet]
        public async Task<ActionResult> Get()
        {

            return Ok(await _context.Suppliers.ToListAsync());


        }

        // Get por parámetro
        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {

            //200 Ok

            var owner = await _context.Suppliers.FirstOrDefaultAsync(x => x.Id == id);

            if (owner == null)
            {


                return NotFound();
            }

            return Ok(owner);


        }


        [HttpPost]
        public async Task<ActionResult> Post(Supplier Supplier)
        {
            _context.Add(Supplier);
            await _context.SaveChangesAsync();
            return Ok(Supplier);
        }

    }
}
