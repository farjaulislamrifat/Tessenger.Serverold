using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using NuGet.Protocol.Plugins;
using Tessenger.Server.Data;
using Tessenger.Server.Models;

namespace Tessenger.Server.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupChatsController : ControllerBase
    {
        private readonly IDbContextFactory< TessengerServerContext> _context;
        private readonly TessengerServerContext tessengerServerContext;
        public GroupChatsController(IDbContextFactory<TessengerServerContext> context, TessengerServerContext tessengerServerContext)
        {
            _context = context;
            this.tessengerServerContext = tessengerServerContext;
        }

        [HttpGet("Gets")]
        public async Task<ActionResult<IEnumerable<GroupChat>>> GetGroupChat()
        {
            return await (await _context.CreateDbContextAsync()).GroupChat.ToListAsync();
        }

        [HttpGet("GetGroupChat/{id}")]
        public async Task<ActionResult<GroupChat>> GetGroupChatById(long id)
        {
            var groupChat =await (await _context.CreateDbContextAsync()).GroupChat.FirstOrDefaultAsync(c => c.Id == id);
            if (groupChat == null)
            {
                return NotFound();
            }

            return Ok(groupChat);
        }
        [HttpGet("GetGroupChat_LastChat_ByGusernameAndSender/{Gusername}")]
        public async Task<ActionResult<GroupChat>> GetGroupChat_LastChat_ByGusernameAndSender(string Gusername)
        {
            var groupChat = (await _context.CreateDbContextAsync()).GroupChat.Where(c => c.GroupUsername == Gusername).ToList().Last();
            if (groupChat == null)
            {
                return NotFound();
            }
            return Ok(groupChat);
        }
        [HttpGet("GetGroupChat_LastChat_ByGusernameAndSender/{Gusername},{SenderUsername}")]
        public async Task<ActionResult<GroupChat>> GetGroupChat_LastChat_ByGusernameAndSender(string Gusername , string SenderUsername)
        {
            var groupChat = (await _context.CreateDbContextAsync()).GroupChat.Where(c => c.GroupUsername == Gusername && c.Sendusername == SenderUsername).ToList().Last();
            if (groupChat == null)
            {
                return NotFound();
            }
            return Ok(groupChat);
        }

        [HttpGet("GetGroupChat_WithCount_ByGusername/{gusername},{start},{count}")]
        public async Task<ActionResult<List<GroupChat>>> GetGroupChat_LastChat_ByGusernameAndSender(string gusername, int start, int count)
        {
            var groupChat = (await _context.CreateDbContextAsync()).GroupChat.Where(c => c.GroupUsername == gusername).Skip(start).Take(count).ToList();
            if (groupChat == null)
            {
                return NotFound();
            }
            return Ok(groupChat);
        }


        [HttpPut("Put/{id}")]
        public async Task<IActionResult> PutGroupChat(long id, GroupChat groupChat)
        {
            if (id != groupChat.Id)
            {
                return BadRequest();
            }

            tessengerServerContext.Entry(groupChat).State = EntityState.Modified;

            try
            {
                await tessengerServerContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupChatExists(id))
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
        public async Task<ActionResult<GroupChat>> PostGroupChat(GroupChat groupChat)
        {
            tessengerServerContext.GroupChat.Add(groupChat);
            await tessengerServerContext.SaveChangesAsync();

            return CreatedAtAction("GetGroupChat", new { id = groupChat.Id }, groupChat);
        }

        [HttpDelete("DeleteGroupChat/{id}")]
        public async Task<IActionResult> DeleteGroupChat(long id)
        {
            var groupChat = await (await _context.CreateDbContextAsync()).GroupChat.FindAsync(id);
            if (groupChat == null)
            {
                return NotFound();
            }

            tessengerServerContext.GroupChat.Remove(groupChat);
            await tessengerServerContext.SaveChangesAsync();

            return NoContent();
        }

        private bool GroupChatExists(long id)
        {
            return (_context.CreateDbContext()).GroupChat.Any(e => e.Id == id);
        }
    }
}
