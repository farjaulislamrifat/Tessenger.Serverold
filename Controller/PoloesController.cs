using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tessenger.Server.Data;
using Tessenger.Server.Models;

namespace Tessenger.Server.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PoloesController : ControllerBase
    {
        private readonly IDbContextFactory<TessengerServerContext> _context;
        private readonly TessengerServerContext tessengerServerContext;
        public PoloesController(IDbContextFactory<TessengerServerContext> context, TessengerServerContext tessengerServerContext)
        {
            _context = context;
            this.tessengerServerContext = tessengerServerContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Polo>>> GetPolo()
        {
            return await (await _context.CreateDbContextAsync()).Polo.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Polo>> GetPolo(int id)
        {
            var polo = await (await _context.CreateDbContextAsync()).Polo.FindAsync(id);

            if (polo == null)
            {
                return NotFound();
            }

            return polo;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPolo(int id, Polo polo)
        {
            if (id != polo.Id)
            {
                return BadRequest();
            }

            tessengerServerContext.Entry(polo).State = EntityState.Modified;

            try
            {
                await tessengerServerContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PoloExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Polo>> PostPolo(Polo polo)
        {
            tessengerServerContext.Polo.Add(polo);
            await tessengerServerContext.SaveChangesAsync();

            return CreatedAtAction("GetPolo", new { id = polo.Id }, polo);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePolo(int id)
        {
            var polo = await (await _context.CreateDbContextAsync()).Polo.FindAsync(id);
            if (polo == null)
            {
                return NotFound();
            }

            tessengerServerContext.Polo.Remove(polo);
            tessengerServerContext.SaveChangesAsync();

            return NoContent();
        }

        private bool PoloExists(int id)
        {
            return (_context.CreateDbContext()).Polo.Any(e => e.Id == id);
        }
    }
}
