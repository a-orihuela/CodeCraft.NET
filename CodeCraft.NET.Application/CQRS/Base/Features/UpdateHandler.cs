using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using CodeCraft.NET.Application.Contracts.Persistence.Base;
using CodeCraft.NET.Application.Resources;
using CodeCraft.NET.Cross.Domain;

namespace CodeCraft.NET.Application.CQRS.Base.Features
{
	public abstract class UpdateHandler<TRequest, TEntity, TUnitOfWork> : BaseHandler<TRequest, TEntity, TUnitOfWork>, IRequestHandler<TRequest, bool>
		where TRequest : class, IRequest<bool>
		where TEntity : BaseDomainModel
		where TUnitOfWork : IUnitOfWork
	{
		public readonly string ERROR_MESSAGE = ResourceAccess.Messages("EntityUpdateError");
		public readonly string SUCCESS_MESSAGE = ResourceAccess.Messages("EntityUpdateSuccess");

		protected UpdateHandler(TUnitOfWork unitOfWork, IMapper mapper, ILogger logger)
			: base(unitOfWork, mapper, logger)
		{
		}

		public virtual async Task<bool> Handle(TRequest request, CancellationToken cancellationToken)
		{
			int id = GetPropertyId(request);
			TEntity entity = await GetEntityById(id);
			_mapper.Map(request, entity, request.GetType(), typeof(TEntity));
			if (await UpdateEntityInRepository(entity) <= 0)
			{
				_logger.LogError(string.Format(ERROR_MESSAGE, typeof(TEntity).Name));
				throw new Exception(string.Format(ERROR_MESSAGE, typeof(TEntity).Name));
			}
			_logger.LogInformation(string.Format(SUCCESS_MESSAGE, id));
			return true;
		}

		private async Task<int> UpdateEntityInRepository(TEntity entity)
		{
			IAsyncRepository<TEntity> repository = _unitOfWork.Repository<TEntity>();
			repository.UpdateEntity(entity);
			return await _unitOfWork.Complete();
		}
	}
}
