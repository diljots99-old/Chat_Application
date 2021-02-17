using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Chat_Application.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the ChatApplicationUser class
    public class ChatApplicationUser : IdentityUser
    {
        [PersonalData]
        public String First_Name { get; set; }
        [PersonalData]
        public String Last_Name { get; set; }
        [PersonalData]
        public DateTime DOB { get; set; }
    }
}
