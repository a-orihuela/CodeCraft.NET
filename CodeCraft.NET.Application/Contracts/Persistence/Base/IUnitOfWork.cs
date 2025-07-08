using CodeCraft.NET.Cross.Domain;

namespace CodeCraft.NET.Application.Contracts.Persistence.Base
{
	public interface IUnitOfWork : IDisposable
	{
		IAsyncRepository<TEntity> Repository<TEntity>() where TEntity : BaseDomainModel;
		Task<int> Complete();
	}
}
