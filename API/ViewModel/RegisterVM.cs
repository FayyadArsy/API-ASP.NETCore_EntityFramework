using API.Models;

namespace API.ViewModel
{
    public class RegisterVM
    {
        public string FirstName {  get; set; }
        public string LastName { get; set; } 
        public string Phone { get; set; }
        public DateTime BirthDate {  get; set; }
        public int Salary { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public string Password { get; set; }
        public Degree Degree {  get; set; }
        public string GPA { get; set; }
        public int University_Id { get; set; }
    }
   
  
}
