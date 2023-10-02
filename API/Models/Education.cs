using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Education
    {
        [Key]
        public int Id { get; set; }
        public Degree Degree { get; set; }
        public string GPA { get; set; }
        public int University_Id { get; set; }
        public University University { get; set; }
        public ICollection<Profilling> Profillings { get; set;}

    }

    public enum Degree
    {
        D3, D4, S1, S2, S3,
    }
}
