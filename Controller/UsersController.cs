using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using Microsoft.Graph.Me;
using Microsoft.Identity.Client;
using Tessenger.Server.Authentication;
using Tessenger.Server.Data;
using Tessenger.Server.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Tessenger.Server.Controller
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : ControllerBase
    {
        private readonly IDbContextFactory<TessengerServerContext> _context;
        private readonly TessengerServerContext tessengerServerContext;


        public UsersController( IDbContextFactory<TessengerServerContext> context, TessengerServerContext tessengerServerContext)
        {
            _context = context;
            this.tessengerServerContext = tessengerServerContext;
        }


        [HttpGet("Gets")]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await (await _context.CreateDbContextAsync()).User.ToListAsync();
        }

        [ServiceFilter(typeof(AuthServiceFillter))]
        [HttpGet("Get/{username}")]
        public async Task<ActionResult<User>> GetUser(string username)
        {
            var user = await (await _context.CreateDbContextAsync()).User.FirstOrDefaultAsync(c => c.Username == username);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }


        [HttpGet("GetUserByUsername/{username},{password}")]
        public async Task<ActionResult<User>> GetUserByUsername(string username, string password)
        {

            var user = await (await _context.CreateDbContextAsync()).User.FirstOrDefaultAsync(c => c.Username == username);
            if (user == null || user.Password != password)
            {
                return NotFound();
            }
            return user;
        }

        [HttpGet("GetUserByEmail/{email},{password}")]
        public async Task<ActionResult<User>> GetUserByEmail(string email, string password)
        {
            var username = (await (await _context.CreateDbContextAsync()).Information.FirstOrDefaultAsync(c => c.Email == email)).Username;
            var user = await (await _context.CreateDbContextAsync()).User.FirstOrDefaultAsync(c => c.Username == username);
            if (user == null || user.Password != password)
            {
                return NotFound();
            }
            return user;
        }

        [ServiceFilter(typeof(AuthServiceFillter))]
        [HttpGet("GetUser_ByEmail/{Email}")]
        public async Task<ActionResult<User>> GetUser_ByEmail(string Email)
        {
            Information information = null;
            var strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                 @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                 @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            var re = new Regex(strRegex);

            if (re.IsMatch(Email))
            {
                information = await (await _context.CreateDbContextAsync()).Information.FirstOrDefaultAsync(c => c.Email == Email);
            }
            if (information == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(await (await _context.CreateDbContextAsync()).User.FirstOrDefaultAsync(c => c.Username == information.Username));
            }
        }

        [ServiceFilter(typeof(AuthServiceFillter))]
        [ServiceFilter(typeof(AuthServiceFillterForTemp))]
        [HttpGet("GetUser_Exists/{username}")]
        public async Task<ActionResult<bool>> GetUser_Exists(string username)
        {
            var User = (await _context.CreateDbContextAsync()).User.Any(c => c.Username == username);
            return Ok(User);
        }

        [ServiceFilter(typeof(AuthServiceFillter))]
        [ServiceFilter(typeof(AuthServiceFillterForTemp))]
        [HttpGet("GetUniqueUsername/{name}")]
        public async Task<ActionResult<string>> GetUser_UniqueUsername(string name)
        {
            int i = 0;
            while (true)
            {
                var user = await (await _context.CreateDbContextAsync()).User.FirstOrDefaultAsync(c => c.Username.ToLower() == $"{name.ToLower()}-{i}");
                if (user == null)
                {
                    return Ok($"{name.ToLower()}-{i}");
                }
                else
                {
                    i++;
                }
            }
        }

        [ServiceFilter(typeof(AuthServiceFillter))]
        [HttpPut("Put/{username}")]
        public async Task<IActionResult> PutUser(string username, User user)
        {
            if (username != user.Username)
            {
                return BadRequest();
            }

            tessengerServerContext.Entry(user).State = EntityState.Modified;
            try
            {
                await tessengerServerContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(username))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok();
        }
        [ServiceFilter(typeof(AuthServiceFillter))]
        [ServiceFilter(typeof(AuthServiceFillterForTemp))]
        [HttpPost("Post")]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            tessengerServerContext.User.Add(user);
            await tessengerServerContext.SaveChangesAsync();
            return CreatedAtAction("GetUser", new { username = user.Username }, user);
        }
        [ServiceFilter(typeof(AuthServiceFillter))]
        [HttpDelete("Delete/{username}")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            var user = await (await _context.CreateDbContextAsync()).User.FirstOrDefaultAsync(c => c.Username == username);
            if (user == null)
            {
                return NotFound();
            }
            tessengerServerContext.User.Remove(user);
            await tessengerServerContext.SaveChangesAsync();
            return Ok();
        }
        private bool UserExists(string username)
        {
            return tessengerServerContext.User.Any(e => e.Username == username);
        }
    }
}
