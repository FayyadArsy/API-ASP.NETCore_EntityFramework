using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Account
    {
        [ForeignKey("NIK")]
        public string NIK { get; set; }
        public string password { get; set; }
        public Employee Employee { get; set; }
        public Profilling Profilling { get; set; }
    }
}
