using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tessenger.Server.Authentication;
using Tessenger.Server.Common;
using Tessenger.Server.Data;
using Tessenger.Server.Models;

namespace Tessenger.Server.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(AuthServiceFillter))]

    public class InformationController : ControllerBase
    {
        private readonly IDbContextFactory<TessengerServerContext> _context;
        private readonly IConfiguration _configuration;

        private readonly TessengerServerContext tessengerServerContext;


        public InformationController(IDbContextFactory<TessengerServerContext> context, IConfiguration configuration, TessengerServerContext tessengerServerContext)
        {
            _context = context;

            _configuration = configuration;
            this.tessengerServerContext = tessengerServerContext;
        }

        [HttpGet("Gets")]
        public async Task<ActionResult<IEnumerable<Information>>> GetInformation()
        {
            return Ok(await (await _context.CreateDbContextAsync()).Information.ToListAsync());
        }

        [HttpGet("Get/{username}")]
        public async Task<ActionResult<Information>> GetInformation(string username)
        {
            var information = await (await _context.CreateDbContextAsync()).Information.FirstOrDefaultAsync(c => c.Username == username);
            if (information == null)
            {
                return NotFound();

            }

           
            return Ok(information);
        }

        [HttpGet("Get_WithArg/{username},{arg}")]
        public async Task<ActionResult<Information>> GetInformation_WithArg(string username, string arg)
        {
            var information = await (await _context.CreateDbContextAsync()).Information.FirstOrDefaultAsync(c => c.Username == username);
            if (information == null)
            {
                return NotFound();
            }
            
            return Ok((Common_Function.ArgInformation(arg, information)));
        }
        [ServiceFilter(typeof(AuthServiceFillterForTemp))]
        [HttpGet("Exists/{username}")]
        public async Task<ActionResult<bool>> GetInformation_Exists(string username)
        {
            var Exists = (await _context.CreateDbContextAsync()).Information.Any(c => c.Username == username);
            return Ok(Exists);
        }

        [ServiceFilter(typeof(AuthServiceFillterForTemp))]
        [HttpGet("Exists_Email/{email}")]
        public async Task<ActionResult<bool>> GetInformation_ExistsEmail(string email)
        {
            var Exists = (await _context.CreateDbContextAsync()).Information.Any(c => c.Email == email);
            return Ok(Exists);
        }

        [HttpGet("GetInformation_WithArg_WithCount_ByUsername/{username},{arg},{start},{count}")]
        public async Task<ActionResult<IEnumerable<Information>>> GetInformation_WithArg_WithCount_ByUsername(string username, string arg, int start, short count)
        {
            var Myinformation = await (await _context.CreateDbContextAsync()).Information.FirstOrDefaultAsync(c => c.Username == username);
            if (Myinformation == null)
            {
                return NotFound();
            }
            List<object> UsernameResults = new List<object>();
            var friends = (await (await _context.CreateDbContextAsync()).Friend.FirstOrDefaultAsync(c => c.Username == username)).Fusername;
            foreach (var friend in friends)
            {
                var Myfriend_Friend = (await (await _context.CreateDbContextAsync()).Friend.FirstOrDefaultAsync(c => c.Username == friend)).Fusername;
                if (Myfriend_Friend != null)
                {
                    foreach (var item in Myfriend_Friend)
                    {
                        UsernameResults.Add(item);
                    }
                }
            }
            if (Myinformation.Address != "")
            {
                var Myaddresss = Myinformation.Address.Split(",");
                foreach (var item in Myaddresss)
                {
                    var AdderssEqualInformations = (await _context.CreateDbContextAsync()).Information.Where(c => c.Address.Contains(item));
                    if (AdderssEqualInformations != null)
                    {
                        foreach (var item2 in AdderssEqualInformations)
                        {
                            UsernameResults.Add(item2.Username);
                        }
                    }
                }
            }

            var alluserList = (await _context.CreateDbContextAsync()).Information.ToList();
            for (int i = 0; i < (alluserList.Count < 100 ? alluserList.Count : 100); i++)
            {
                var rend = new Random().Next(0, alluserList.Count - 1);
                UsernameResults.Add(alluserList[i].Username);
            }

            UsernameResults = UsernameResults.Distinct().ToList();
            var request = await (await _context.CreateDbContextAsync()).Request.FirstOrDefaultAsync(x => x.Username == username);
            foreach (var item in request.GetRequsteduser)
            {
                UsernameResults.Remove(item);
            }
            foreach (var item in request.SendRequsteduser)
            {
                UsernameResults.Remove(item);
            }

            UsernameResults.Remove(Myinformation.Username);
            UsernameResults = await Common.Common_Function.ListRandomizeAsync(UsernameResults);
            UsernameResults = UsernameResults.Skip(start).Take(count).ToList();
            List<Information> informations = new();
            foreach (var item in UsernameResults)
            {
                try
                {
                    var info = ((await _context.CreateDbContextAsync()).Information.FirstOrDefault(c => c.Username == item.ToString()));
                    var information = Common_Function.ArgInformation(arg, info);
                    informations.Add(information);
                }
                catch (Exception)
                {

                }
            }
            return Ok(informations);
        }

        [HttpPut("Put/{username}")]
        public async Task<IActionResult> PutInformation(string username, Information information)
        {
            if (username != information.Username)
            {
                return BadRequest();
            }
            tessengerServerContext.Entry(information).State = EntityState.Modified;
            try
            {
                await tessengerServerContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InformationExists(username))
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
        public async Task<ActionResult<Information>> PostInformation(Information information)
        {
            tessengerServerContext.Information.Add(information);
            try
            {
                await tessengerServerContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (InformationExists(information.Username))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtAction("GetInformation", new { id = information.Username }, information);
        }

        [HttpDelete("Delete/{username}")]
        public async Task<IActionResult> DeleteInformation(string username)
        {
            var information = await (await _context.CreateDbContextAsync()).Information.FirstOrDefaultAsync(c => c.Username == username);
            if (information == null)
            {
                return NotFound();
            }
            tessengerServerContext.Information.Remove(information);
            await tessengerServerContext.SaveChangesAsync();
            return NoContent();
        }

        private bool InformationExists(string id)
        {
            return (_context.CreateDbContext()).Information.Any(e => e.Username == id);
        }
    }
}
