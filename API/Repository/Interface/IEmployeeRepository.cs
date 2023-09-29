using API.Models;

namespace API.Repository.Interface
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> Get();
        Employee Get(string NIK);
        int Insert(Employee employee);
        int Update(Employee employee);
        int Delete(string NIK);
    }
}
