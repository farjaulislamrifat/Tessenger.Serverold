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
    public class FriendsController : ControllerBase
    {
        private readonly IDbContextFactory<TessengerServerContext> _context;
        private readonly TessengerServerContext tessengerServerContext;
        public FriendsController(IDbContextFactory<TessengerServerContext> context, TessengerServerContext tessengerServerContext)
        {
            _context = context;
            this.tessengerServerContext = tessengerServerContext;
        }

        [HttpGet("Gets")]
        public async Task<ActionResult<IEnumerable<Friend>>> GetFriend()
        {
            return await (await _context.CreateDbContextAsync()).Friend.ToListAsync();
        }

        [HttpGet("Get/{username}")]
        public async Task<ActionResult<Friend>> GetFriend(string username)
        {
            var friend = await (await _context.CreateDbContextAsync()).Friend.FirstOrDefaultAsync(c => c.Username == username);
            if (friend == null)
            {
                return NotFound();
            }
            return Ok(friend);
        }

        [HttpGet("GetFriend_WithCount_ByUsername/{username},{start},{count}")]
        public async Task<ActionResult<List<Friend>>> GetFriend_WithCount_ByUsername(string username, int start, short count)
        {
            var friend = (await _context.CreateDbContextAsync()).Friend.Where(c => c.Username == username).ToList();
            if (friend == null)
            {
                return NotFound();
            }
            return Ok(friend);
        }

        [HttpGet("Get_FriendBlockeds/{username}")]
        public async Task<ActionResult<List<string>>> GetFriendBlocks(string username)
        {
            var BlockedUsername = (await (await _context.CreateDbContextAsync()).Friend.FirstOrDefaultAsync(c => c.Username == username)).Blocked;
            if (BlockedUsername != null)
            {
                return NotFound();
            }
            return Ok(BlockedUsername);
        }

        [HttpGet("Get_FriendStatus/{username}")]
        public async Task<ActionResult<List<string>>> GetFriendStatus(string username)
        {
            var StatusUsername = (await (await _context.CreateDbContextAsync()).Friend.FirstOrDefaultAsync(c => c.Username == username)).StatusAccess;
            if (StatusUsername != null)
            {
                return NotFound();
            }
            return Ok(StatusUsername);
        }
        [ServiceFilter(typeof(AuthServiceFillterForTemp))]
        [HttpGet("Exists/{username}")]
        public async Task<ActionResult<bool>> GetFriendExists(string username)
        {
            var Exists = (await _context.CreateDbContextAsync()).Friend.Any(Friend => Friend.Username == username);
            return Ok(Exists);
        }

        [HttpGet("Get_BlockedExists/{username},{fusername}")]
        public async Task<ActionResult<bool>> GetFriendIsBlockedExists(string username, string fusername)
        {
            var IsBlocked = (await (await _context.CreateDbContextAsync()).Friend.FirstOrDefaultAsync(c => c.Username == username)).Blocked.Contains(fusername);
            return Ok(IsBlocked);
        }

        [HttpGet("Get_StatusAccessExists/{username},{fusername}")]
        public async Task<ActionResult<bool>> GetFriendIsStatusExists(string username, string fusername)
        {
            var IsBlocked = (await (await _context.CreateDbContextAsync()).Friend.FirstOrDefaultAsync(c => c.Username == username)).StatusAccess.Contains(fusername);
            return Ok(IsBlocked);
        }


        [HttpPut("Put/{username}")]
        public async Task<IActionResult> PutFriend(string username, Friend friend)
        {
            if (username != friend.Username)
            {
                return BadRequest();
            }
            tessengerServerContext.Entry(friend).State = EntityState.Modified;
            try
            {
                await tessengerServerContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FriendExists(username))
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
        public async Task<ActionResult<Friend>> PostFriend(Friend friend)
        {
            tessengerServerContext.Friend.Add(friend);
            await tessengerServerContext.SaveChangesAsync();
            return CreatedAtAction("GetFriend", new { id = friend.Id }, friend);
        }

        [HttpDelete("Delete/{username}")]
        public async Task<IActionResult> DeleteFriend(string username)
        {
            var friend = await (await _context.CreateDbContextAsync()).Friend.FirstOrDefaultAsync(c => c.Username == username);
            if (friend == null)
            {
                return NotFound();
            }
            tessengerServerContext.Friend.Remove(friend);
            await tessengerServerContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("DeleteFriend/{username},{fusername}")]
        public async Task<IActionResult> DeleteFriend(string username, string fusername)
        {
            var friend = await (await _context.CreateDbContextAsync()).Friend.FirstOrDefaultAsync(c => c.Username == username);
            if (friend == null)
            {
                return NotFound();
            }
            friend.Fusername.Remove(fusername);
            tessengerServerContext.Entry(friend).State = EntityState.Modified;
            try
            {
                await tessengerServerContext.SaveChangesAsync();
            }
            catch (Exception)
            {
            }
            return Ok();
        }

        [HttpDelete("DeleteFriendBlocked/{username},{fusername}")]
        public async Task<IActionResult> DeleteFriendBlocked(string username, string fusername)
        {
            var friend = await (await _context.CreateDbContextAsync()).Friend.FirstOrDefaultAsync(c => c.Username == username);
            if (friend == null)
            {
                return NotFound();
            }
            friend.Blocked.Remove(fusername);
            tessengerServerContext.Entry(friend).State = EntityState.Modified;
            try
            {
                await tessengerServerContext.SaveChangesAsync();
            }
            catch (Exception)
            {

            }
            return Ok();
        }

        [HttpDelete("DeleteFriendStatus/{username},{fusername}")]
        public async Task<IActionResult> DeleteFriendStatus(string username, string fusername)
        {
            var friend = await (await _context.CreateDbContextAsync()).Friend.FirstOrDefaultAsync(c => c.Username == username);
            if (friend == null)
            {
                return NotFound();
            }
            friend.StatusAccess.Remove(fusername);
            tessengerServerContext.Entry(friend).State = EntityState.Modified;
            try
            {
                await tessengerServerContext.SaveChangesAsync();
            }
            catch (Exception)
            {

            }
            return Ok();
        }

        private bool FriendExists(string username)
        {
            return ( _context.CreateDbContext()).Friend.Any(e => e.Username == username);
        }
    }
}
