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
    public class RequestsController : ControllerBase
    {
        private readonly IDbContextFactory<TessengerServerContext> _context;
        private readonly TessengerServerContext tessengerServerContext;
        public RequestsController(IDbContextFactory<TessengerServerContext> context, TessengerServerContext tessengerServerContext)
        {
            _context = context;
            this.tessengerServerContext = tessengerServerContext;
        }

        [HttpGet("Gets")]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequest()
        {
            return await (await _context.CreateDbContextAsync()).Request.ToListAsync();
        }

        [HttpGet("Get/{username}")]
        public async Task<ActionResult<Request>> GetRequest(string username)
        {
            var request = await (await _context.CreateDbContextAsync()).Request.FirstOrDefaultAsync(c=> c.Username ==username);
            if (request == null)
            {
                return NotFound();
            }
            return request;
        }
        [ServiceFilter(typeof(AuthServiceFillterForTemp))]
        [HttpGet("Exists/{username}")]
        public async Task<ActionResult<bool>> GetRequest_Exists(string username)
        {
            var request = (await _context.CreateDbContextAsync()).Request.Any(c => c.Username == username);
            if (request == null)
            {
                return NotFound();
            }
            return request;
        }

        [HttpGet("Get_GetRequest_ByUsername_WithCount/{username},{start},{count}")]
        public async Task<ActionResult<List<UserRequest>>> Get_GetRequest_ByUsername_WithCount(string username, int start, short count)
        {
            var request = await (await _context.CreateDbContextAsync()).Request.FirstOrDefaultAsync(c => c.Username == username);
            if (request == null)
            {
                return NotFound();
            }
            var userRequestList = new List<UserRequest>();
            foreach (var item in request.GetRequsteduser)
            {
                var information = await (await _context.CreateDbContextAsync()).Information.FirstOrDefaultAsync(c=> c.Username == item);
                userRequestList.Add(new UserRequest
                {
                    ImageUrl = information.ProfileImage,
                    Name = information.Name,
                    Time = request.GetTime[request.GetRequsteduser.IndexOf(item)],
                    Username = item
                });
            }
            return userRequestList;
        }

        [HttpGet("Get_SendRequest_ByUsername_WithCount/{username},{start},{count}")]
        public async Task<ActionResult<List<UserRequest>>> Get_SendRequest_ByUsername_WithCount(string username, int start, short count)
        {
            var request = await (await _context.CreateDbContextAsync()).Request.FirstOrDefaultAsync(c => c.Username == username);
            if (request == null)
            {
                return NotFound();
            }
            var userRequestList = new List<UserRequest>();
            foreach (var item in request.SendRequsteduser)
            {
                var information = await (await _context.CreateDbContextAsync()).Information.FirstOrDefaultAsync(c=> c.Username == item);
                userRequestList.Add(new UserRequest
                {                                                               
                    ImageUrl = information.ProfileImage,
                    Name = information.Name,
                    Time = request.SendTime[request.SendRequsteduser.IndexOf(item)],
                    Username = item
                });
            }
            return userRequestList;
        }

        [HttpPut("Put/{username}")]
        public async Task<IActionResult> PutRequest(string username, Request request)
        {
            if (username != request.Username)
            {
                return BadRequest();
            }
            tessengerServerContext.Entry(request).State = EntityState.Modified;
            try
            {
                await tessengerServerContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(username))
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
        [ServiceFilter(typeof(AuthServiceFillterForTemp))]
        [HttpPost("Post")]
        public async Task<ActionResult<Request>> PostRequest(Request request)
        {
            tessengerServerContext.Request.Add(request);
            await tessengerServerContext.SaveChangesAsync();
            return CreatedAtAction("GetRequest", new { id = request.Id }, request);
        }

        [HttpDelete("Delete/{username}")]
        public async Task<IActionResult> DeleteRequest(string username)
        {
            var request = await (await _context.CreateDbContextAsync()).Request.FirstOrDefaultAsync(c => c.Username == username);
            if (request == null)
            {
                return NotFound();
            }
            tessengerServerContext.Request.Remove(request);
            await tessengerServerContext.SaveChangesAsync();
            return NoContent();
        }
        
        [HttpDelete("Delete_GetRequesteduser/{username},{fusername}")]
        public async Task<IActionResult> DeleteGetRequsteduser(string username, string fusername)
        {
            var request = await (await _context.CreateDbContextAsync()).Request.FirstOrDefaultAsync(c=> c.Username == username);
            if (request == null)
            {
                return NotFound();
            }
            var index = request.GetRequsteduser.IndexOf(fusername);
            request.GetRequsteduser.Remove(fusername);
            request.GetTime.RemoveAt(index);
            tessengerServerContext.Entry(request).State = EntityState.Modified;
            try
            {
                await tessengerServerContext.SaveChangesAsync();
            }
            catch (Exception)
            {
            }
            return NoContent();
        }    
        
        [HttpDelete("Delete_sendRequesteduser/{username},{fusername}")]
        public async Task<IActionResult> DeletesendRequsteduser(string username, string fusername)
        {
            var request = await (await _context.CreateDbContextAsync()).Request.FirstOrDefaultAsync(c => c.Username == username);
            if (request == null)
            {
                return NotFound();
            }
            var index = request.SendRequsteduser.IndexOf(fusername);
            request.SendRequsteduser.Remove(fusername);
            request.SendTime.RemoveAt(index);
            tessengerServerContext.Entry(request).State = EntityState.Modified;
            try
            {
                await tessengerServerContext.SaveChangesAsync();
            }
            catch (Exception)
            {
            }
            return NoContent();
        }

        private bool RequestExists(string username)
        {
            return tessengerServerContext.Request.Any(e => e.Username == username);
        }
    }
}
