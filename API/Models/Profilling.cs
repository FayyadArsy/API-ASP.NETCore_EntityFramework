using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Profilling
    {

        /*[ForeignKey("NIK")]*/
        [Key]
        public string NIK { get; set; }
    
        public Education Education { get; set; }
    }
}
