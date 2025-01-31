using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Models;
using System;
using System.Threading.Tasks;
using Tessenger.Server.Authentication;
using Tessenger.Server.Data;
using Tessenger.Server.Models;


namespace Tessenger.Server.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(AuthServiceFillter))]
    public class CallRoomController : ControllerBase
    {

        private readonly IDbContextFactory<TessengerServerContext> _context;
        private readonly IConfiguration _configuration;
        private readonly TessengerServerContext tessengerServerContext;

        public CallRoomController(IDbContextFactory<TessengerServerContext> context, IConfiguration configuration, TessengerServerContext tessengerServerContext)
        {
            _context = context;
            _configuration = configuration;
            this.tessengerServerContext = tessengerServerContext;
        }

        [HttpGet("Get")]
        public  ActionResult Get()
        {
            return Ok(_context.CreateDbContext().CallRoomRequest);
        }

      
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult> Get(int id)
        {
            return Ok( await _context.CreateDbContext().CallRoomRequest.FindAsync(id));

        }  

        [HttpGet("GetByRoomid/{Roomid}")]
        public  ActionResult Get(string Roomid)
        {
            return Ok( _context.CreateDbContext().CallRoomRequest.FirstOrDefault(c=> c.RoomID == Roomid));

        }

 
        [HttpPost("Post")]
        public async Task<ActionResult> Post(CallRoom value)
        {
            tessengerServerContext.CallRoomRequest.Add(value);
            
            await tessengerServerContext.SaveChangesAsync();
            return CreatedAtAction("GetChat", new { id = value.Id }, value);
        }

      
        [HttpPut("Put/{id}")]
        public async Task<ActionResult> Put(int id, CallRoom value)
        {

            if (id != value.Id)
            {
                return BadRequest();
            }
            tessengerServerContext.Entry(value).State = EntityState.Modified;
            try
            {
                await tessengerServerContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
               
            }
            return NoContent();
        }


        [HttpDelete("DeleteById/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var room = await(await _context.CreateDbContextAsync()).CallRoomRequest.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            tessengerServerContext.CallRoomRequest.Remove(room);
            await tessengerServerContext.SaveChangesAsync();
            return NoContent();
        }    
        [HttpDelete("DeleteByRoomId/{id}")]
        public async Task<ActionResult> Delete(string roomid)
        {
            var call = (await _context.CreateDbContextAsync()).CallRoomRequest.FirstOrDefault(c=> c.RoomID == roomid);
            if (call == null)
            {
                return NotFound();
            }
            tessengerServerContext.CallRoomRequest.Remove(call);
            await tessengerServerContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
