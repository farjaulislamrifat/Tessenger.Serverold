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
    public class GroupsController : ControllerBase
    {
        private readonly IDbContextFactory<TessengerServerContext> _context;
        private readonly TessengerServerContext tessengerServerContext;
        public GroupsController(IDbContextFactory<TessengerServerContext> context, TessengerServerContext tessengerServerContext)
        {
            _context = context;
            this.tessengerServerContext = tessengerServerContext;
        }

        [HttpGet("Gets")]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroup()
        {
            return await (await _context.CreateDbContextAsync()).Group.ToListAsync();
        }


        [HttpGet("GetGroup/{Gusername}")]
        public async Task<ActionResult<Group>> GetGroup(string Gusername)
        {
            var @group = await (await _context.CreateDbContextAsync()).Group.FirstOrDefaultAsync(c => c.Username == Gusername);

            if (@group == null)
            {
                return NotFound();
            }
            return Ok(@group);
        }
        [HttpGet("GetGroup_UniqueUsername/{name}")]
        public async Task<ActionResult<string>> GetGroup_UniqueUsername(string name)
        {
            int i = 0;
            while (true)
            {
                var user = await (await _context.CreateDbContextAsync()).Group.FirstOrDefaultAsync(c => c.Username.ToLower() == $"{name.ToLower()}-{i}");
                if (user == null)
                {
                    return $"{name.ToLower()}-{i}" is string model ? Ok(model) : NotFound();
                }
                else
                {
                    i++;
                }

            }
        }
        [HttpGet("GetAdminGroup/{username},{start},{count}")]
        public async Task<ActionResult<List<GroupAdminList>>> GetAdminGroup(string username, int start, short count)
        {
            var groupList = (await _context.CreateDbContextAsync()).Group.Where(c => c.Admin == username).Skip(start).Take(count);
            List<GroupAdminList> groupAdminList = new();
            foreach (var item in groupList)
            {
                var info = await (await _context.CreateDbContextAsync()).GroupInformation.FirstOrDefaultAsync(c => c.Username == item.Username);
                groupAdminList.Add(new GroupAdminList
                {
                    Description = info.Description,
                    GroupName = info.Name,
                    GroupUsername = item.Username,
                    ImageUrl = info.ImageUrl
                });
            }
            return Ok(groupAdminList);
        }

        [HttpGet("GetGroup_WithArg_ByUsername/{gusername},{arg}")]
        public async Task<ActionResult<Group>> GetGroup_WithArg_ByUsername(string gusername, string arg)
        {
            var group = await (await _context.CreateDbContextAsync()).Group.FirstOrDefaultAsync(c => c.Username == gusername);
            return Ok(Common.Common_Function.ArgGroup(arg, group));
        }

        [HttpGet("GetGroup_Exists_ByUsername/{gusername}")]
        public async Task<ActionResult<bool>> GetGroup_Exists_ByUsername(string gusername)
        {
            var groupExists = (await _context.CreateDbContextAsync()).Group.Any(c => c.Username == gusername);
            return Ok(groupExists);
        }

        [HttpGet("GetGroups_WithCount_WithArg_ByUsername/{username},{start},{count}")]
        public async Task<ActionResult<List<GroupAdminList>>> GetGroup_WithCount_WithArg_ByUsername(string username, int start, short count)
        {
            var groupList = (await _context.CreateDbContextAsync()).Group.Where(c => c.Members.Contains(username)).Skip(start).Take(count);
            List<GroupAdminList> groupAdminList = new();
            foreach (var item in groupList)
            {
                var info = await (await _context.CreateDbContextAsync()).GroupInformation.FirstOrDefaultAsync(c => c.Username == item.Username);
                groupAdminList.Add(new GroupAdminList
                {
                    Description = info.Description,
                    GroupName = info.Name,
                    GroupUsername = item.Username,
                    ImageUrl = info.ImageUrl
                });
            }
            return Ok(groupAdminList);
        }

        [HttpPut("Put/{gusername}")]
        public async Task<IActionResult> PutGroup(string gusername, Group @group)
        {
            if (gusername != @group.Username)
            {
                return BadRequest();
            }
            tessengerServerContext.Entry(@group).State = EntityState.Modified;
            try
            {
                await tessengerServerContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(gusername))
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
        public async Task<ActionResult<Group>> PostGroup(Group @group)
        {
            tessengerServerContext.Group.Add(@group);
            await tessengerServerContext.SaveChangesAsync();
            return CreatedAtAction("GetGroup", new { id = @group.Id }, @group);
        }

        [HttpDelete("DeleteGroup/{gusername}")]
        public async Task<IActionResult> DeleteGroup(string gusername)
        {
            var @group = await (await _context.CreateDbContextAsync()).Group.FirstOrDefaultAsync(c => c.Username == gusername);
            if (@group == null)
            {
                return NotFound();
            }
            tessengerServerContext.Group.Remove(@group);
            await tessengerServerContext.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("DeleteGroup_MemberByGusername/{gusername},{MemberUsername}")]
        public async Task<IActionResult> DeleteGroup(string gusername, string MemberUsername)
        {
            var @group = await (await _context.CreateDbContextAsync()).Group.FirstOrDefaultAsync(c => c.Username == gusername);
            if (@group == null)
            {
                return NotFound();
            }
            group.Members.Remove(MemberUsername);
            tessengerServerContext.Entry(@group).State = EntityState.Modified;
            try
            {
                await tessengerServerContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(gusername))
                {
                    return NotFound();
                }
                else
                {

                }
            }
            return NoContent();
        }

        private bool GroupExists(string guser)
        {
            return (_context.CreateDbContext()).Group.Any(e => e.Username == guser);
        }
    }
}
