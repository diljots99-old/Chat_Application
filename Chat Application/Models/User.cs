using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat_Application.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace Chat_Application.Models
{
    public class User
    {
       
        public String Id { get; set; }
        public String First_Name { get; set; }
        public String Last_Name { get; set; }
        public DateTime DOB { get; set; }
        public string UserName { get; set; }
        public  string Email { get; set; }

        public  string PhoneNumber { get; set; }

        public ICollection<Participants> participant { get; set; }
    }
}
