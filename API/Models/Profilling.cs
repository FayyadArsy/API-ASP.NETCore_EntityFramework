using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Profilling
    {

        /*[ForeignKey("NIK")]*/
        [Key]
        public string NIK { get; set; }
        public int Education_id { get; set; }  
        public Account Account { get; set; }
        public Education Education { get; set; }
    }
}
