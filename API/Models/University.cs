

using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class University
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public ICollection<Education> Educations { get; set; }
    }
}
