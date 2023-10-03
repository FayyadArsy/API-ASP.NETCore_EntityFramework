using API.Models;
using API.ViewModel;

namespace API.Repository.Interface
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> Get();
        Employee Get(string NIK);
        int Insert(Employee employee);
        int Update(Employee employee);
        int Delete(string NIK);
        int Register(RegisterVM register);
        IEnumerable<GetVM> Getv2();
    }
}
