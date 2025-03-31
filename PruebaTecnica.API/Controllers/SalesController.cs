using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaTecnica.API.Data;
using PruebaTecnica.Shared.Entities;
using System.Security.Claims;
using System.Threading.Tasks;


namespace PruebaTecnica.API.Controllers

{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] //protegidos
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
        public async Task<ActionResult> Get([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {

            var userId = User.FindFirst("UserId")?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userId == null || userRole == null)
            {
                return Unauthorized("No se pudo determinar la identidad del usuario.");
            }

            IQueryable<Sale> salesQuery = _context.Sales;

            if (userRole != "Admin") // Si no es Admin, filtrar por UserId
            {
                salesQuery = salesQuery.Where(s => s.UserId == userId);
            }

            if (startDate.HasValue && endDate.HasValue)
            {
                salesQuery = salesQuery.Where(s => s.SaleDate >= startDate.Value && s.SaleDate <= endDate.Value);
            }

            var sales = await salesQuery.ToListAsync();
            return Ok(sales);



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

            // Obtener el ID del usuario autenticado desde los claims
            var userIdClaim = User.FindFirst("UserId");

            if (userIdClaim == null)
            {
                return Unauthorized("No se pudo obtener el ID del usuario.");
            }

            sale.UserId = (userIdClaim.Value).ToString();

            try
            {
                _context.Add(sale);
                await _context.SaveChangesAsync();
                return Ok(sale);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.ToString());
                return null;
            }

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
