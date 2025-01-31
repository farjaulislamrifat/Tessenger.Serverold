using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public class ChatsController : ControllerBase
    {
        private readonly IDbContextFactory<TessengerServerContext> _context;
        private readonly IConfiguration _configuration;
        private readonly TessengerServerContext tessengerServerContext;

        public ChatsController(IDbContextFactory<TessengerServerContext> context, IConfiguration configuration, TessengerServerContext tessengerServerContext)
        {
            _context = context;
            _configuration = configuration;
            this.tessengerServerContext = tessengerServerContext;
        }

        [HttpGet("Gets")]
        public async Task<ActionResult<IEnumerable<Chat>>> GetChat()
        {
            return await (await _context.CreateDbContextAsync()).Chat.ToListAsync();
        }

        [HttpGet("Get/{id}")]
        public async Task<ActionResult<Chat>> GetChat(long id)
        {
            var chat = await (await _context.CreateDbContextAsync()).Chat.FindAsync(id);

            if (chat == null)
            {
                return NotFound();
            }
            return chat;
        }

        [HttpGet("GetChats_WithCount_ByUsernameAndFusername/{username},{fusername},{start},{count}")]
        public async Task<ActionResult<List<ChatClientObject>>> GetChats_WithCount_ByUsernameAndFusername(string username, string fusername, int start, short count)
        {
            var chat = (_context.CreateDbContext()).Chat.Where(c => c.Username == username && c.Fusername == fusername).ToList();
            if (chat == null || chat.Count() == 0)
            {
                return NotFound();
            }
            chat.Reverse();
            chat = chat.Skip(start).Take(count).ToList();
            chat.Reverse();

            var chatclientobj = new List<ChatClientObject>();
            var tasks = new List<Task>();

            foreach (var item in chat)
            {

                tasks.Add(Task.Run(async () =>
                {

                    var replyChat = item.ReplyMessId == "" ? new Chat
                    {
                        Message = "",
                        Documentfiles = new List<string>(),
                        VoicRecoardFile = "",
                        Images = new List<string>(),
                        Id = -1
                    } : (_context.CreateDbContext()).Chat.FirstOrDefault(c => c.Id == Convert.ToInt64(item.ReplyMessId)) is Chat model ? model : new Chat
                    {
                        Message = "",
                        Documentfiles = new List<string>(),
                        VoicRecoardFile = "",
                        Images = new List<string>(),
                        Id = -1
                    };
                    var info = (await (_context.CreateDbContext()).Information.ToListAsync()).FirstOrDefault(c => c.Username == item.Sendusername);

                    var documentpropertylist = new List<DocumentProperty>();

                    var Documenttasks = new List<Task>();

                    foreach (var item_ in item.Documentfiles)
                    {
                        Documenttasks.Add(Task.Run(async () =>
                        {
                            var documentinfo = (await (_context.CreateDbContext()).DocumentDn.ToListAsync()).FirstOrDefault(c => c.Uniquename == Path.GetFileName(item_));

                            string filesize = "";
                            var Length = Convert.ToInt64(documentinfo.Size);

                            if (Convert.ToInt32((Length / 1024)) > 0 && Convert.ToInt32((Length / 1024)) < 1024)
                            {
                                filesize = (Length / 1024).ToString() + "Kb";
                            }
                            else if (Convert.ToInt32((Length / 1000000)) > 0)
                            {
                                filesize = (Length / 1000000).ToString() + "Mb";
                            }
                            else
                            {
                                filesize = Length + "b";
                            }
                            documentpropertylist.Add(new DocumentProperty { Name = documentinfo.Name, Size = filesize, Uri = item_, Type = Path.GetExtension(item_) });

                        }));
                    }

                    await Task.WhenAll(Documenttasks);

                    chatclientobj.Add(new ChatClientObject
                    {
                        Id = item.Id,
                        Documentfiles = documentpropertylist,
                        Images = item.Images,
                        Message = item.Message,
                        ReactEmoji = item.ReactEmoji,
                        Senderusername = item.Sendusername,
                        ReplyHasFile = replyChat.Documentfiles.Count == 0 ? false : true,
                        ReplyHasVoice = replyChat.VoicRecoardFile != "" ? true : false,
                        ReplyMessae = replyChat.Message,
                        VoicRecoardFile = item.VoicRecoardFile,
                        ReplyImages = replyChat.Images,
                        SenderProfileImgae = info.ProfileImage,
                        Status = item.Status,
                        Time = item.Time,
                        SenderName = info.Name,
                        ReplyId = replyChat.Id

                    });

                }));
            }

            await Task.WhenAll(tasks);

            chatclientobj = chatclientobj.OrderBy(c => c.Id).ToList();

            return Ok(chatclientobj);
        }

        [HttpGet("GetChat_Last_ByUsernameAndFusername/{username},{fusername}")]
        public async Task<ActionResult<Chat>> GetChat_Last_ByUsernameAndFusername(string username, string fusername)
        {
            var chat = (await _context.CreateDbContextAsync()).Chat.Where(c => c.Username == username && c.Fusername == fusername).ToList();

            if (chat.Count() != 0)
            {

                var chatlast = chat.Count() != 0 ? chat.Last() : null;


                return Ok(chatlast);
            }
            return NoContent();
        }




        [HttpGet("GetChat_ChatClientObject_ById/{id}")]
        public async Task<ActionResult<ChatClientObject>> GetChat_ChatClientObject_Byid(long id)
        {
            var item = (await _context.CreateDbContextAsync()).Chat.FirstOrDefault(c => c.Id == id);

            var chatclientobj = new ChatClientObject();
            if (item != null)
            {

                var replyChat = (await (await _context.CreateDbContextAsync()).Chat.FirstOrDefaultAsync(c => c.Id == Convert.ToInt64(item.ReplyMessId == "" ? "-1" : item.ReplyMessId))) is Chat model ? model : new Chat
                {
                    Message = "",
                    Documentfiles = new List<string>(),
                    VoicRecoardFile = "",
                    Images = new List<string>(),
                };
                var info = await (await _context.CreateDbContextAsync()).Information.FirstOrDefaultAsync(c => c.Username == item.Sendusername);

                var documentpropertylist = new List<DocumentProperty>();
                foreach (var item_ in item.Documentfiles)
                {
                    var documentinfo = await (await _context.CreateDbContextAsync()).DocumentDn.FirstOrDefaultAsync(c => c.Uniquename == Path.GetFileName(item_));

                    string filesize = "";
                    var Length = Convert.ToInt64(documentinfo.Size);

                    if (Convert.ToInt32((Length / 1024)) > 0 && Convert.ToInt32((Length / 1024)) < 1024)
                    {
                        filesize = (Length / 1024).ToString() + "Kb";
                    }
                    else if (Convert.ToInt32((Length / 1000000)) > 0)
                    {
                        filesize = (Length / 1000000).ToString() + "Mb";
                    }
                    else
                    {
                        filesize = Length + "b";
                    }
                    documentpropertylist.Add(new DocumentProperty { Name = documentinfo.Name, Size = filesize, Uri = item_, Type = Path.GetExtension(item_) });

                }

                chatclientobj = new ChatClientObject
                {
                    Id = item.Id,
                    Documentfiles = documentpropertylist,
                    Images = item.Images,
                    Message = item.Message,
                    ReactEmoji = item.ReactEmoji,
                    Senderusername = item.Sendusername,
                    ReplyHasFile = replyChat.Documentfiles.Count == 0 ? false : true,
                    ReplyHasVoice = replyChat.VoicRecoardFile != "" ? true : false,
                    ReplyMessae = replyChat.Message,
                    VoicRecoardFile = replyChat.VoicRecoardFile,
                    ReplyImages = replyChat.Images,
                    SenderProfileImgae = info.ProfileImage,
                    Status = item.Status,
                    Time = item.Time,
                    SenderName = info.Name
                };

            }
            return Ok(chatclientobj);
        }

        [HttpPut("Put/{id}")]
        public async Task<IActionResult> PutChat(long id, Chat chat)
        {
            if (id != chat.Id)
            {
                return BadRequest();
            }
            tessengerServerContext.Entry(chat).State = EntityState.Modified;
            try
            {
                await tessengerServerContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChatExists(id))
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
        public async Task<ActionResult<Chat>> PostChat(Chat chat)
        {
            tessengerServerContext.Chat.Add(chat);
            await tessengerServerContext.SaveChangesAsync();
            return CreatedAtAction("GetChat", new { id = chat.Id }, chat);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteChat(long id)
        {
            var chat = await (await _context.CreateDbContextAsync()).Chat.FindAsync(id);
            if (chat == null)
            {
                return NotFound();
            }
            tessengerServerContext.Chat.Remove(chat);
            await tessengerServerContext.SaveChangesAsync();
            return NoContent();
        }

        private bool ChatExists(long id)
        {
            return (_context.CreateDbContext()).Chat.Any(e => e.Id == id);
        }
    }
}
