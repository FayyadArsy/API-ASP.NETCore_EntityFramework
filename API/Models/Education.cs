using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Education
    {
        [Key]
        public int Id { get; set; }
        public string? Degree { get; set; }
        public string? GPA { get; set; }
/*degree jadikan ennum, d3, s1,s2,s3*/
        public University University { get; set; }
        public ICollection<Profilling> Profillings { get; set;}

    }
}
