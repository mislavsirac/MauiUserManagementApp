using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace MauiUserManagementApp.Models
{
    public class User
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        [ForeignKey("Country")]
        public int CountryID { get; set; }
        public byte[] ProfileImage { get; set; }

    }
}
