using PatientManager.Core.Application.Interfaces.Repositories;
using PatientManager.Core.Application.Interfaces.Services;
using PatientManager.Core.Application.ViewModels.LaboratoryTests;
using PatientManager.Core.Domain.Entities;

namespace PatientManager.Core.Application.Services
{
    public class LaboratoryTestService : ILaboratoryTestService
    {
        private readonly ILaboratoryTestRepository _laboratoryTestRepository;
        private readonly IUserSessionService _userSessionService;

        public LaboratoryTestService(ILaboratoryTestRepository laboratoryTestRepository, IUserSessionService userSessionService)
        {
            _laboratoryTestRepository = laboratoryTestRepository;
            _userSessionService = userSessionService;
        }

        public async Task<LaboratoryTestSaveViewModel> AddAsync(LaboratoryTestSaveViewModel saveViewModel, int consultoryId)
        {
            var laboratoryTest = new LaboratoryTest
            {
                Name = saveViewModel.Name,
                ConsultoryId = consultoryId
            };

            laboratoryTest = await _laboratoryTestRepository.AddAsync(laboratoryTest);
            saveViewModel.Id = laboratoryTest.Id;

            return saveViewModel;
        }

        public async Task UpdateAsync(LaboratoryTestSaveViewModel saveViewModel)
        {
            var existingTest = await _laboratoryTestRepository.GetByIdAsync(saveViewModel.Id);
            existingTest.Name = saveViewModel.Name;

            await _laboratoryTestRepository.UpdateAsync(existingTest);
        }

        public async Task RemoveAsync(int id)
        {
            await _laboratoryTestRepository.RemoveAsync(await _laboratoryTestRepository.GetByIdAsync(id));
        }

        public async Task<List<LaboratoryTestViewModel>> GetAllAsync()
        {
            var laboratoryTests = await _laboratoryTestRepository.GetAllAsync();

            return laboratoryTests.Where(l => l.ConsultoryId == _userSessionService.GetUserInSession("admin").ConsultoryId).Select(test => new LaboratoryTestViewModel
            {
                Id = test.Id,
                Name = test.Name
            }).ToList();
        }

        public async Task<LaboratoryTestSaveViewModel> GetByIdAsync(int id)
        {
            var test = await _laboratoryTestRepository.GetByIdAsync(id);

            return new LaboratoryTestSaveViewModel
            {
                Id = test.Id,
                Name = test.Name
            };
        }

        public async Task<List<LaboratoryTestViewModel>> GetAllAsyncForAssistant()
        {
            var laboratoryTests = await _laboratoryTestRepository.GetAllAsync();

            return laboratoryTests.Where(l => l.ConsultoryId == _userSessionService.GetUserInSession("assis").ConsultoryId).Select(test => new LaboratoryTestViewModel
            {
                Id = test.Id,
                Name = test.Name
            }).ToList();
        }
    }
}
