using PatientManager.Core.Application.Interfaces.Repositories;
using PatientManager.Core.Application.Interfaces.Services;
using PatientManager.Core.Application.ViewModels.LaboratoryResults;

namespace PatientManager.Core.Application.Services
{
    public class LaboratoryResultService : ILaboratoryResultService
    {
        private readonly ILaboratoryResultRepository _laboratoryResultRepository;
        private readonly IUserSessionService _userSessionService;

        public LaboratoryResultService(ILaboratoryResultRepository laboratoryResultRepository, IUserSessionService userSessionService)
        {
            _laboratoryResultRepository = laboratoryResultRepository;
            _userSessionService = userSessionService;
        }

        public async Task<List<LaboratoryResultViewModel>> GetAllPendingResultsAsync(string? patientIdCard)
        {
            var results = await _laboratoryResultRepository.GetAllWithIncludeAsync(new List<string> { "LaboratoryTest", "Patient" });

            var pendingResults = results
                .Where(r => r.ConsultoryId == _userSessionService.GetUserInSession("assis").ConsultoryId && !r.IsCompleted)
                .Where(r => string.IsNullOrEmpty(patientIdCard) || r.Patient.IdCard == patientIdCard)
                .Select(r => new LaboratoryResultViewModel
                {
                    Id = r.Id,
                    PatientName = $"{r.Patient.FirstName} {r.Patient.LastName}",
                    PatientIdCard = r.Patient.IdCard,
                    TestName = r.LaboratoryTest.Name,
                    IsCompleted = r.IsCompleted
                })
                .ToList();

            return pendingResults;
        }

        public async Task<LaboratoryResultViewModel> GetByIdAsync(int id)
        {
            var result = await _laboratoryResultRepository.GetByIdAsync(id);

            return new LaboratoryResultViewModel
            {
                Id = result.Id,
                TestName = result.LaboratoryTest.Name,
                PatientName = $"{result.Patient.FirstName} {result.Patient.LastName}",
                Result = result.Result,
                IsCompleted = result.IsCompleted
            };
        }

        public async Task ReportResultAsync(int id, string result)
        {
            var labResult = await _laboratoryResultRepository.GetByIdAsync(id);
            if (labResult != null)
            {
                labResult.Result = result;
                labResult.IsCompleted = true;
                await _laboratoryResultRepository.UpdateAsync(labResult);
            }
        }
    }
}
