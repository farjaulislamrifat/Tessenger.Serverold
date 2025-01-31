using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tessenger.Server.Authentication;
using Tessenger.Server.Data;
using Tessenger.Server.Models;

namespace Tessenger.Server.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(AuthServiceFillter))]
    public class DocumentDnsController : ControllerBase
    {
        private readonly IDbContextFactory< TessengerServerContext>  _context;
        private readonly TessengerServerContext tessengerServerContext;
        public DocumentDnsController(IDbContextFactory<TessengerServerContext> context, TessengerServerContext tessengerServerContext)
        {
            _context = context;
            this.tessengerServerContext = tessengerServerContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocumentDn>>> GetDocumentDn()
        {
            return Ok(await (await _context.CreateDbContextAsync()).DocumentDn.ToListAsync());
        }

        [HttpGet("Get/{uniquename}")]
        public async Task<ActionResult<DocumentDn>> GetDocumentDn(string uniquename)
        {
            var documentDn = await (await _context.CreateDbContextAsync()).DocumentDn.FirstOrDefaultAsync(c => c.Uniquename == uniquename);
            if (documentDn == null)
            {
                return NotFound();
            }
            return Ok(documentDn);
        }

        [HttpPut("Put/{uniquename}")]
        public async Task<IActionResult> PutDocumentDn(string uniquename, DocumentDn documentDn)
        {
            if (uniquename != documentDn.Uniquename)
            {
                return BadRequest();
            }
            tessengerServerContext.Entry(documentDn).State = EntityState.Modified;
            try
            {
                await tessengerServerContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentDnExists(uniquename))
                {
                    return NotFound();
                }
                else
                {
                }
            }
            return NoContent();
        }

        [HttpPost("Post")]
        public async Task<ActionResult<DocumentDn>> PostDocumentDn(DocumentDn documentDn)
        {
            tessengerServerContext.DocumentDn.Add(documentDn);
            await tessengerServerContext.SaveChangesAsync();
            return CreatedAtAction("GetDocumentDn", new { id = documentDn.Id }, documentDn);
        }

        [HttpDelete("Delete/{uniquename}")]
        public async Task<IActionResult> DeleteDocumentDn(string uniquename)
        {
            var documentDn = await (await _context.CreateDbContextAsync()).DocumentDn.FirstOrDefaultAsync(c => c.Uniquename == uniquename);
            if (documentDn == null)
            {
                return NotFound();
            }
            tessengerServerContext.DocumentDn.Remove(documentDn);
            await tessengerServerContext.SaveChangesAsync();
            return NoContent();
        }

        private bool DocumentDnExists(string uniquename)
        {
            return ( _context.CreateDbContext()).DocumentDn.Any(e => e.Uniquename == uniquename);
        }
    }
}
