using Microsoft.AspNetCore.Identity;
using System;

namespace deneme33.Data.Entities
{
    public class AppUser:IdentityUser
    {
        public string FullName { get; set; }

        public DateTime DateModified { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
