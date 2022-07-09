using System.Threading.Tasks;

namespace CalixtoStore.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}