using API.Models;

namespace API.Repository.Interface
{
    public interface IUniversityRepository
    {
        IEnumerable<University> Get();
        University Get(int Id);
        int Insert(University university);
        int Update(University university);
        int Delete(int Id);
    }
}
