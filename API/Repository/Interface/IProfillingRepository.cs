using API.Models;

namespace API.Repository.Interface
{
    public interface IProfillingRepository
    {
        IEnumerable<Profilling> Get();
        Profilling Get(string NIK);
        int Insert(Profilling profilling);
        int Update(Profilling profilling);
        int Delete(string NIK);
    }
}
