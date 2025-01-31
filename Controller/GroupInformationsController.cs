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
    public class GroupInformationsController : ControllerBase
    {
        private readonly IDbContextFactory<TessengerServerContext> _context;
        private readonly TessengerServerContext tessengerServerContext;
        public GroupInformationsController(IDbContextFactory<TessengerServerContext> context, TessengerServerContext tessengerServerContext)
        {
            _context = context;
            this.tessengerServerContext = tessengerServerContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupInformation>>> GetGroupInformation()
        {
            return await (await _context.CreateDbContextAsync()).GroupInformation.ToListAsync();
        }

        [HttpGet("GetGroupInformation/{gusername}")]
        public async Task<ActionResult<GroupInformation>> GetGroupInformation(string gusername)
        {
            var groupInformation = await (await _context.CreateDbContextAsync()).GroupInformation.FirstOrDefaultAsync(c => c.Username == gusername);

            if (groupInformation == null)
            {
                return NotFound();
            }

            return Ok(groupInformation);
        }

        [HttpGet("GetGroupInformation_Exists_ByGusername/{gusername}")]
        public async Task<ActionResult<bool>> GetGroupInformation_Exists_ByGusername(string gusername)
        {
            var groupInformation = (await _context.CreateDbContextAsync()).GroupInformation.Any(c => c.Username == gusername);

            if (groupInformation == null)
            {
                return NotFound();
            }

            return Ok(groupInformation);
        }
        
        [HttpGet("GetGroupInformation_WithArg_ByGusername/{gusername},{arg}")]
        public async Task<ActionResult<GroupInformation>> GetGroupInformation_Exists_ByGusername(string gusername, string arg)
        {
            var groupInformation = await (await _context.CreateDbContextAsync()).GroupInformation.FirstOrDefaultAsync(c => c.Username == gusername);

            if (groupInformation == null)
            {
                return NotFound();
            }
            return Ok(Common.Common_Function.GroupInformationArg(arg, groupInformation));
        } 
        
     

        [HttpPut("Put/{gusername}")]
        public async Task<IActionResult> PutGroupInformation(string gusername, GroupInformation groupInformation)
        {
            if (gusername != groupInformation.Username)
            {
                return BadRequest();
            }

            tessengerServerContext.Entry(groupInformation).State = EntityState.Modified;

            try
            {
                await tessengerServerContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupInformationExists(gusername))
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

        [HttpPost("Post")]
        public async Task<ActionResult<GroupInformation>> PostGroupInformation(GroupInformation groupInformation)
        {
            tessengerServerContext.GroupInformation.Add(groupInformation);
            await tessengerServerContext.SaveChangesAsync();
            return CreatedAtAction("GetGroupInformation", new { id = groupInformation.Id }, groupInformation);
        }

        [HttpDelete("DeleteGroupInformation_ByGusername/{gusername}")]
        public async Task<IActionResult> DeleteGroupInformation(string gusername)
        {
            var groupInformation = await (await _context.CreateDbContextAsync()).GroupInformation.FirstOrDefaultAsync(c=> c.Username == gusername);
            if (groupInformation == null)
            {
                return NotFound();
            }
            tessengerServerContext.GroupInformation.Remove(groupInformation);
            await tessengerServerContext.SaveChangesAsync();
            return NoContent();
        }

        private bool GroupInformationExists(string gusername)
        {
            return (_context.CreateDbContext()).GroupInformation.Any(e => e.Username == gusername);
        }
    }
}
