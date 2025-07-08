using PatientManager.Core.Application.ViewModels.LaboratoryResults;

namespace PatientManager.Core.Application.Interfaces.Services
{
    public interface ILaboratoryResultService
    {
        Task<List<LaboratoryResultViewModel>> GetAllPendingResultsAsync(string? patientIdCard);
        Task<LaboratoryResultViewModel> GetByIdAsync(int id);
        Task ReportResultAsync(int id, string result);
    }
}
