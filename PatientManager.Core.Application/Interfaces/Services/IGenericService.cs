namespace PatientManager.Core.Application.Interfaces.Services
{
    public interface IGenericService<SaveViewModel, ViewModel>
        where SaveViewModel : class
        where ViewModel : class
    {
        Task<SaveViewModel> AddAsync(SaveViewModel createViewModel, int consultoryId);
        Task<SaveViewModel> GetByIdAsync(int id);
        Task<List<ViewModel>> GetAllAsync();
        Task RemoveAsync(int id);
    }
}
