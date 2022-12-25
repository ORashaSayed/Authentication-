using System.Threading.Tasks;

namespace JWT.Business
{
    public interface IUnitOfWork
    {
        Task<bool> SaveChangesAsync();
    }
}
