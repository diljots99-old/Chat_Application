using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Chat_Application.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
namespace Chat_Application.Models
{
    public class DatabaseContext: DbContext
    {
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Participants> Participants { get; set; }
        public DbSet<Messages>  Messages { get; set; }
        public DbSet<User>  Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //options.UseSqlite("Data Source=chat.db");

            options.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\diljo\\source\\repos\\Chat Application\\Database\\DataBB.mdf\";Integrated Security=True;Connect Timeout=30");

        }
    }
}
