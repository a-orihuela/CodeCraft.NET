using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using CodeCraft.NET.Application.Contracts.Persistence.Base;
using CodeCraft.NET.Application.Middleware.Exceptions;
using CodeCraft.NET.Application.Resources;
using CodeCraft.NET.Cross.Domain;

namespace CodeCraft.NET.Application.CQRS.Base.Features
{
	public abstract class CreateHandler<TRequest, TEntity, TUnitOfWork> : BaseHandler<TRequest, TEntity, TUnitOfWork>, IRequestHandler<TRequest, int>
		where TRequest : class, IRequest<int>
		where TEntity : BaseDomainModel
		where TUnitOfWork : IUnitOfWork
	{
		public readonly string ERROR_MESSAGE = ResourceAccess.Messages("EntityCreatedError");
		public readonly string SUCCESS_MESSAGE = ResourceAccess.Messages("EntityCreatedSuccess");

		protected CreateHandler(TUnitOfWork unitOfWork, IMapper mapper, ILogger logger)
			: base(unitOfWork, mapper, logger)
		{
		}

		public virtual async Task<int> Handle(TRequest request, CancellationToken cancellationToken)
		{
			TEntity entity = _mapper.Map<TEntity>(request);
			if (await AddEntityToRepository(entity) <= 0)
			{
				_logger.LogError(string.Format(ERROR_MESSAGE, typeof(TEntity).Name));
				throw new Exception(string.Format(ERROR_MESSAGE, typeof(TEntity).Name));
			}

			System.Reflection.PropertyInfo? idProperty = entity.GetType().GetProperty("Id");
			if (idProperty == null)
			{
				_logger.LogError(ERROR_MESSAGE_ID_PROPERTY);
				throw new NotFoundPropertyException(typeof(TEntity).Name, "Id");
			}
			int id = (int)idProperty.GetValue(entity);

			_logger.LogInformation(string.Format(SUCCESS_MESSAGE, typeof(TEntity).Name, id));
			return id;
		}

		private async Task<int> AddEntityToRepository(TEntity entity)
		{
			IAsyncRepository<TEntity> repository = _unitOfWork.Repository<TEntity>();
			repository.AddEntity(entity);
			return await _unitOfWork.Complete();
		}
	}
}
