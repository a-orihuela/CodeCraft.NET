using AutoMapper;
using MediatR;
using CodeCraft.NET.Application.Contracts.Persistence.Base;
using CodeCraft.NET.Application.CQRS.Base.Specifications;
using CodeCraft.NET.Cross.Domain;

namespace CodeCraft.NET.Application.CQRS.Base.Queries
{
	public abstract class PaginationHandler<TRequest, TEntity, TUnitOfWork, TSpecParams> : IRequestHandler<TRequest, Pagination<TEntity>>
			where TRequest : PaginationBaseQuery, IRequest<Pagination<TEntity>>
			where TEntity : BaseDomainModel
			where TUnitOfWork : IUnitOfWork
			where TSpecParams : SpecificationParams, new()
	{
		protected readonly TUnitOfWork _unitOfWork;
		protected readonly IMapper _mapper;

		protected PaginationHandler(TUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<Pagination<TEntity>> Handle(TRequest request, CancellationToken cancellationToken)
		{
			TSpecParams specParams = CreateSpecificationParams(request);
			IReadOnlyList<TEntity> data = await _unitOfWork.Repository<TEntity>().GetAllWithSpecifications(CreateSpecification(specParams));
			int totalData = await _unitOfWork.Repository<TEntity>().CountAsync(CreateCountingSpecification(specParams));
			int totalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(totalData) / Convert.ToDecimal(request.PageSize)));

			return new Pagination<TEntity>
			{
				Count = totalData,
				Data = data,
				PageCount = totalPages,
				PageIndex = request.PageIndex,
				PageSize = request.PageSize
			};
		}

		protected abstract ISpecification<TEntity> CreateSpecification(TSpecParams specParams);
		protected abstract ISpecification<TEntity> CreateCountingSpecification(TSpecParams specParams);
		protected abstract TSpecParams CreateSpecificationParams(TRequest request);
	}
}
