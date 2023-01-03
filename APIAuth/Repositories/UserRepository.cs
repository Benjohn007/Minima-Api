using APIAuth.Models;

namespace APIAuth.Repositories
{
    public class UserRepository
    {
        public static List<User> Users = new()
        {
            new()
            {
                UserName = "Decagon",
                EmailAddress = "DecagonEmail",
                Password ="DecagonPass",
                Surname = "Software",
                GivenName = "Engineer",
                Role = "Admin"
            },

            new()
            {
                UserName = "Tom",
                EmailAddress = "TomEmail",
                Password ="TomPass",
                Surname = "Tom",
                GivenName = "Femi",
                Role = "Standard"
            },
             new()
            {
                UserName = "Wariz",
                EmailAddress = "WarizEmail",
                Password ="WarizPass",
                Surname = "Wariz",
                GivenName = "Salami",
                Role = "Standard"
            },
        };
    }
}
