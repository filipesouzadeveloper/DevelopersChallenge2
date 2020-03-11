using System.Threading.Tasks;

namespace DevChallenge.Import.OFX.Api.Domain.Contracts
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task InserirAsync(TEntity entity);
    }
}
