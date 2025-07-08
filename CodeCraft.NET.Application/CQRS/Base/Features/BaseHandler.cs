using AutoMapper;
using Microsoft.Extensions.Logging;
using CodeCraft.NET.Application.Contracts.Persistence.Base;
using CodeCraft.NET.Application.Middleware.Exceptions;
using CodeCraft.NET.Application.Resources;
using CodeCraft.NET.Cross.Domain;

namespace CodeCraft.NET.Application.CQRS.Base.Features
{
	public abstract class BaseHandler<TRequest, TEntity, TUnitOfWork>
		where TRequest : class
		where TEntity : BaseDomainModel
		where TUnitOfWork : IUnitOfWork
	{
		protected readonly TUnitOfWork _unitOfWork;
		protected readonly IMapper _mapper;
		protected readonly ILogger _logger;

		protected readonly string ERROR_MESSAGE_ID_NOTEXISTS = ResourceAccess.Messages("EntityErrorIdNotExist");
		protected readonly string ERROR_MESSAGE_ID_PROPERTY = ResourceAccess.Messages("EntityErrorIdProperty");

		protected BaseHandler(TUnitOfWork unitOfWork, IMapper mapper, ILogger logger)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_logger = logger;
		}

		protected int GetPropertyId(TRequest request)
		{
			System.Reflection.PropertyInfo? requestIdProperty = request.GetType().GetProperty("Id");
			if (requestIdProperty == null)
			{
				_logger.LogError(ERROR_MESSAGE_ID_PROPERTY);
				throw new NotFoundPropertyException(typeof(TEntity).Name, "Id");
			}
			return (int)requestIdProperty.GetValue(request);
		}

		protected async Task<TEntity> GetEntityById(int id)
		{
			TEntity entity = await GetEntityByIdInRepository(id);
			if (entity == null)
			{
				_logger.LogError(string.Format(ERROR_MESSAGE_ID_NOTEXISTS, id));
				throw new NotFoundException(typeof(TEntity).Name, id);
			}
			return entity;
		}

		private async Task<TEntity> GetEntityByIdInRepository(int id)
		{
			IAsyncRepository<TEntity> repository = _unitOfWork.Repository<TEntity>();
			return await repository.GetByIdAsync(id);
		}
	}
}
