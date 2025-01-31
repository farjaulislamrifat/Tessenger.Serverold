using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tessenger.Server.Models;

namespace Tessenger.Server.Data
{
    public class TessengerServerContext : DbContext
    {
        public TessengerServerContext (DbContextOptions<TessengerServerContext> options)
            : base(options)
        {
        }

        public DbSet<Tessenger.Server.Models.Chat> Chat { get; set; } = default!;
        public DbSet<Tessenger.Server.Models.DocumentDn> DocumentDn { get; set; } = default!;
        public DbSet<Tessenger.Server.Models.Friend> Friend { get; set; } = default!;
        public DbSet<Tessenger.Server.Models.Group> Group { get; set; } = default!;
        public DbSet<Tessenger.Server.Models.GroupChat> GroupChat { get; set; } = default!;
        public DbSet<Tessenger.Server.Models.GroupInformation> GroupInformation { get; set; } = default!;
        public DbSet<Tessenger.Server.Models.Information> Information { get; set; } = default!;
        public DbSet<Tessenger.Server.Models.Polo> Polo { get; set; } = default!;
        public DbSet<Tessenger.Server.Models.Request> Request { get; set; } = default!;
        public DbSet<Tessenger.Server.Models.User> User { get; set; } = default!;
        public DbSet<Tessenger.Server.Models.GroupRequest> GroupRequest { get; set; } = default!;
        public DbSet<Tessenger.Server.Models.CallRoom> CallRoomRequest { get; set; } = default!;
    }
}
