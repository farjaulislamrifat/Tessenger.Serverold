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
    public class GroupRequestsController : ControllerBase
    {
        private readonly IDbContextFactory<TessengerServerContext> _context;
        private readonly TessengerServerContext tessengerServerContext;
        public GroupRequestsController(IDbContextFactory<TessengerServerContext> context, TessengerServerContext tessengerServerContext)
        {
            _context = context;
            this.tessengerServerContext = tessengerServerContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupRequest>>> GetGroupRequest()
        {
            return await (await _context.CreateDbContextAsync()).GroupRequest.ToListAsync();
        }

        [HttpGet("GetGroup_GetRequestUsers/{username}")]
        public async Task<ActionResult<List<GroupGetRequest>>> GetGroup_GetRequestUsers(string username)
        {
            var groupRequest = (await _context.CreateDbContextAsync()).GroupRequest.Where(c => c.GetRequstedGroupuser.Contains(username));
            List<GroupGetRequest> groupGetRequests = new();
            if (groupRequest == null)
            {
                return NotFound();
            }

            foreach (var item in groupRequest)
            {

                var groupinformation = (await _context.CreateDbContextAsync()).GroupInformation.FirstOrDefault(c => c.Username == item.GroupUsername);

                groupGetRequests.Add(new GroupGetRequest { GroupName = groupinformation.Name, GroupUsername = item.GroupUsername, ImageUrl = groupinformation.ImageUrl, Time = item.GetTime[item.GetRequstedGroupuser.IndexOf(username)], username = username });



            }

            return Ok(groupGetRequests);
        }
        [HttpGet("GetGroup_SendRequestUsers/{Adminusername}")]
        public async Task<ActionResult<List<GroupSendRequest>>> GetGroup_SendRequestUsers(string Adminusername)
        {
            var groupRequest = (await _context.CreateDbContextAsync()).Group.Where(c => c.Admin == Adminusername);

          
                List<GroupSendRequest> groupreqsend = new List<GroupSendRequest>();

            if (groupRequest == null)
            {
                return NotFound();
            }
            foreach (var item in groupRequest)
            {
                var groupRequestuser =await (await _context.CreateDbContextAsync()).GroupRequest.FirstOrDefaultAsync(c => c.GroupUsername == item.Username);


                var groupname = (await _context.CreateDbContextAsync()).GroupInformation.ToList().FirstOrDefault(c => c.Username == item.Username).Name;

                for (int i = 0; i < groupRequestuser.SendRequstedGroupuser.Count; i++)
                {
                    groupreqsend.Add(new GroupSendRequest { GroupName = groupname, GroupUsername = item.Username, Members = groupRequestuser.SendRequstedGroupuser[i], Time = groupRequestuser.SendTime[i], username = Adminusername });

                }
            }
            return Ok(groupreqsend);
        }

        [HttpPut("Put/{gusername}")]
        public async Task<IActionResult> PutGroupRequest(string gusername, GroupRequest groupRequest)
        {
            if (gusername != groupRequest.GroupUsername)
            {
                return BadRequest();
            }

            tessengerServerContext.Entry(groupRequest).State = EntityState.Modified;

            try
            {
                await tessengerServerContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupRequestExists(gusername))
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
        public async Task<ActionResult<GroupRequest>> PostGroupRequest(GroupRequest groupRequest)
        {
            tessengerServerContext.GroupRequest.Add(groupRequest);
            await tessengerServerContext.SaveChangesAsync();
            return CreatedAtAction("GetGroupRequest", new { id = groupRequest.Id }, groupRequest);
        }

        [HttpDelete("DeleteGroupRequest/{gusername}")]
        public async Task<IActionResult> DeleteGroupRequest(string gusername)
        {
            var groupRequest = await (await _context.CreateDbContextAsync()).GroupRequest.FirstOrDefaultAsync(c=> c.GroupUsername == gusername);
            if (groupRequest == null)
            {
                return NotFound();
            }
            tessengerServerContext.GroupRequest.Remove(groupRequest);
            await tessengerServerContext.SaveChangesAsync();

            return NoContent();
        }   
        
        [HttpDelete("DeleteGroupRequest_Get_SendMember/{gusername},{username}")]
        public async Task<IActionResult> DeleteGroupRequest(string gusername , string username)
        {
            var groupRequest = await (await _context.CreateDbContextAsync()).GroupRequest.FirstOrDefaultAsync(c=> c.GroupUsername == gusername);
            if (groupRequest == null)
            {
                return NotFound();
            }

            groupRequest.GetTime.RemoveAt(groupRequest.GetRequstedGroupuser.IndexOf(username));
            groupRequest.GetRequstedGroupuser.Remove(username);
            groupRequest.SendTime.RemoveAt(groupRequest.SendRequstedGroupuser.IndexOf(username));
            groupRequest.SendRequstedGroupuser.Remove(username);
            tessengerServerContext.Entry(groupRequest).State = EntityState.Modified;

            try
            {
                await tessengerServerContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupRequestExists(gusername))
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

        private bool GroupRequestExists(string gusername)
        {
            return (_context.CreateDbContext()).GroupRequest.Any(e => e.GroupUsername == gusername );
        }
    }
}
