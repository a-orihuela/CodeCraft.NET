using CodeCraft.NET.Domain.Model;

namespace CodeCraft.NET.Application.Contracts.Persistence.Repositories
{
	public partial interface IUserRepository
	{
		Task<User> GetByIdentityIdAsync(string identityId, CancellationToken cancellationToken = default);

		Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
	}
}
