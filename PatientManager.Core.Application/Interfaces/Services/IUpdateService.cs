namespace PatientManager.Core.Application.Interfaces.Services
{
    public interface IUpdateService<SaveViewModel> where SaveViewModel : class
    {
        Task UpdateAsync(SaveViewModel saveViewModel);
    }
}
