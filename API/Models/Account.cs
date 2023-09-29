using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Account
    {
        [Key]
        [ForeignKey("profilling")]
        public string NIK { get; set; }
        public string password { get; set; }
     public Profilling profilling { get; set; }
    }
}
