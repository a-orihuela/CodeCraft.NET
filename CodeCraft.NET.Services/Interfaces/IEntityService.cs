using CodeCraft.NET.Services.Models;

namespace CodeCraft.NET.Services.Interfaces
{
    public interface IEntityService<TCreateCommand, TUpdateCommand, TDto, TWithRelatedDto>
        where TCreateCommand : class
        where TUpdateCommand : class
        where TDto : class
        where TWithRelatedDto : class
    {
        Task<ServiceResult<int>> CreateAsync(TCreateCommand command);
        Task<ServiceResult<bool>> UpdateAsync(TUpdateCommand command);
        Task<ServiceResult<bool>> DeleteAsync(int id);
        Task<ServiceResult<TDto?>> GetByIdAsync(int id);
        Task<ServiceResult<TWithRelatedDto?>> GetWithRelatedAsync(int id);
        Task<ServiceResult<IEnumerable<TDto>>> GetAllAsync();
    }
}