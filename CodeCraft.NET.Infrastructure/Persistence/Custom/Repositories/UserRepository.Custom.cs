using Microsoft.EntityFrameworkCore;
using CodeCraft.NET.Domain.Model;

namespace CodeCraft.NET.Infrastructure.Persistence.Repositories
{
	public partial class UserRepository
	{
		public async Task<User> GetByIdentityIdAsync(string identityId, CancellationToken cancellationToken = default)
		{
			return await Context.Set<User>()
				.AsNoTracking()
				.FirstOrDefaultAsync(u => u.IdentityId == identityId, cancellationToken);

		}
	}
}
