using MediatR;
using Microsoft.Extensions.Logging;
using CodeCraft.NET.Services.Models;
using CodeCraft.NET.Services.Interfaces;

namespace CodeCraft.NET.Services.Base
{
    public abstract class BaseEntityService<TCreateCommand, TUpdateCommand, TDto, TWithRelatedDto> 
        : IEntityService<TCreateCommand, TUpdateCommand, TDto, TWithRelatedDto>
        where TCreateCommand : class
        where TUpdateCommand : class
        where TDto : class
        where TWithRelatedDto : class
    {
        protected readonly IMediator _mediator;
        protected readonly ILogger _logger;
        protected readonly string _entityName;

        protected BaseEntityService(IMediator mediator, ILogger logger, string entityName)
        {
            _mediator = mediator;
            _logger = logger;
            _entityName = entityName;
        }

        public virtual async Task<ServiceResult<int>> CreateAsync(TCreateCommand command)
        {
            try
            {
                _logger.LogInformation("Creating new {EntityName}", _entityName);
                
                var result = await _mediator.Send(command);
                var id = Convert.ToInt32(result);
                
                _logger.LogInformation("Successfully created {EntityName} with ID: {Id}", _entityName, id);
                return ServiceResult<int>.Success(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create {EntityName}", _entityName);
                return ServiceResult<int>.Failure($"Failed to create {_entityName}: {ex.Message}");
            }
        }

        public virtual async Task<ServiceResult<bool>> UpdateAsync(TUpdateCommand command)
        {
            try
            {
                _logger.LogInformation("Updating {EntityName}", _entityName);
                
                var result = await _mediator.Send(command);
                var success = Convert.ToBoolean(result);
                
                _logger.LogInformation("Successfully updated {EntityName}", _entityName);
                return ServiceResult<bool>.Success(success);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update {EntityName}", _entityName);
                return ServiceResult<bool>.Failure($"Failed to update {_entityName}: {ex.Message}");
            }
        }

        public virtual async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting {EntityName} with ID: {Id}", _entityName, id);
                
                var deleteCommand = CreateDeleteCommand(id);
                var result = await _mediator.Send(deleteCommand);
                var success = Convert.ToBoolean(result);
                
                _logger.LogInformation("Successfully deleted {EntityName} with ID: {Id}", _entityName, id);
                return ServiceResult<bool>.Success(success);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete {EntityName} with ID: {Id}", _entityName, id);
                return ServiceResult<bool>.Failure($"Failed to delete {_entityName}: {ex.Message}");
            }
        }

        public virtual async Task<ServiceResult<TDto?>> GetByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Getting {EntityName} by ID: {Id}", _entityName, id);
                
                var query = CreateGetByIdQuery(id);
                var result = await _mediator.Send(query);
                var entity = result as TDto;
                
                if (entity != null)
                {
                    _logger.LogInformation("Successfully retrieved {EntityName} with ID: {Id}", _entityName, id);
                    return ServiceResult<TDto?>.Success(entity);
                }
                else
                {
                    _logger.LogWarning("{EntityName} with ID: {Id} was not found", _entityName, id);
                    return ServiceResult<TDto?>.Failure($"{_entityName} with ID {id} not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get {EntityName} with ID: {Id}", _entityName, id);
                return ServiceResult<TDto?>.Failure($"Failed to get {_entityName}: {ex.Message}");
            }
        }

        public virtual async Task<ServiceResult<TWithRelatedDto?>> GetWithRelatedAsync(int id)
        {
            try
            {
                _logger.LogInformation("Getting {EntityName} with related data by ID: {Id}", _entityName, id);
                
                var query = CreateGetWithRelatedQuery(id);
                var result = await _mediator.Send(query);
                var entity = result as TWithRelatedDto;
                
                if (entity != null)
                {
                    _logger.LogInformation("Successfully retrieved {EntityName} with related data for ID: {Id}", _entityName, id);
                    return ServiceResult<TWithRelatedDto?>.Success(entity);
                }
                else
                {
                    _logger.LogWarning("{EntityName} with related data for ID: {Id} was not found", _entityName, id);
                    return ServiceResult<TWithRelatedDto?>.Failure($"{_entityName} with ID {id} not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get {EntityName} with related data for ID: {Id}", _entityName, id);
                return ServiceResult<TWithRelatedDto?>.Failure($"Failed to get {_entityName} with related data: {ex.Message}");
            }
        }

        public virtual async Task<ServiceResult<IEnumerable<TDto>>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Getting all {EntityName}s", _entityName);
                
                // Note: This is a placeholder implementation
                // In a real scenario, you would implement a GetAll query
                var entities = new List<TDto>();
                
                _logger.LogInformation("Retrieved {Count} {EntityName}s", entities.Count, _entityName);
                return ServiceResult<IEnumerable<TDto>>.Success(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get all {EntityName}s", _entityName);
                return ServiceResult<IEnumerable<TDto>>.Failure($"Failed to get {_entityName}s: {ex.Message}");
            }
        }

        // Abstract methods that derived classes must implement
        protected abstract object CreateDeleteCommand(int id);
        protected abstract object CreateGetByIdQuery(int id);
        protected abstract object CreateGetWithRelatedQuery(int id);
    }
}