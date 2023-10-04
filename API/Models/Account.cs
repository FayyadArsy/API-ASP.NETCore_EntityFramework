using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Account
    {
        [Key]
        public string NIK { get; set; }
        public string password { get; set; }
        public string? otp {  get; set; }
        public DateTime? otp_expire {  get; set; }
        public Employee Employee { get; set; }
        public Profilling Profilling { get; set; }

    }
}
