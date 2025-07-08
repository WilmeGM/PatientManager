using PatientManager.Core.Application.ViewModels.LaboratoryTests;

namespace PatientManager.Core.Application.Interfaces.Services
{
    public interface ILaboratoryTestService : IGenericService<LaboratoryTestSaveViewModel, LaboratoryTestViewModel>, IUpdateService<LaboratoryTestSaveViewModel>
    {
        Task<List<LaboratoryTestViewModel>> GetAllAsyncForAssistant();
    }
}
