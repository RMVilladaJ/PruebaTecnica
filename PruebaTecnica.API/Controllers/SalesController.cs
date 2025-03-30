using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaTecnica.API.Data;
using PruebaTecnica.Shared.Entities;
using System.Threading.Tasks;


namespace PruebaTecnica.API.Controllers

{
    [ApiController]
    [Route("/api/Sales")]
    public class SalesController: ControllerBase
    {
        private readonly DataContext _context;

        public SalesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {

            return Ok(await _context.Sales.ToListAsync());


        }

        // Get por parámetro
        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            var owner = await _context.Sales.FirstOrDefaultAsync(x => x.Id == id);

            if (owner == null)
            {
                return NotFound();
            }
            return Ok(owner);

        }


        [HttpPost]
        public async Task<ActionResult> Post(Sale sale)
        {
            _context.Add(sale);
            await _context.SaveChangesAsync();
            return Ok(sale);
        }

        [HttpPut]
        public async Task<ActionResult> Put(Sale sale)
        {
            _context.Update(sale);
            await _context.SaveChangesAsync();
            return Ok(sale);
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var FilasAfectadas = await _context.Sales
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
