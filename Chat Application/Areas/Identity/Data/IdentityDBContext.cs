using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat_Application.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chat_Application.Data
{
    public class IdentityDBContext : IdentityDbContext<ChatApplicationUser>
    {
        //public IdentityDBContext(DbContextOptions<IdentityDBContext> options)
        //    : base(options)
        //{
        //}


        public DbSet<ChatApplicationUser> ChatApplicationUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //options.UseSqlite("Data Source=chat.db");
               options.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\diljo\\source\\repos\\Chat Application\\Database\\DataBB.mdf\";Integrated Security=True;Connect Timeout=30");


        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
