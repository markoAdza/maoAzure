using Microsoft.AspNetCore.Identity;

namespace web.Models
{

    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Building { get; set; }

    }



}