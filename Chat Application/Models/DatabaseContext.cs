using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
namespace Chat_Application.Models
{
    public class DatabaseContext: DbContext
    {
        public DbSet<Conversation> conversations { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=blogging.db");
          
         }
}
}
