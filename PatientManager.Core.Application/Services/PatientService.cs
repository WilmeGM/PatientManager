using PatientManager.Core.Application.Interfaces.Repositories;
using PatientManager.Core.Application.Interfaces.Services;
using PatientManager.Core.Application.ViewModels.Patients;
using PatientManager.Core.Domain.Entities;

namespace PatientManager.Core.Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IUserSessionService _userSessionService;

        public PatientService(IPatientRepository patientRepository, IUserSessionService userSessionService)
        {
            _patientRepository = patientRepository;
            _userSessionService = userSessionService;
        }

        public async Task<PatientSaveViewModel> AddAsync(PatientSaveViewModel saveViewModel, int consultoryId)
        {
            var patient = new Patient
            {
                FirstName = saveViewModel.Firstname,
                LastName = saveViewModel.Lastname,
                Phone = saveViewModel.Phone,
                Address = saveViewModel.Address,
                IdCard = saveViewModel.IdCard,
                BirthDate = saveViewModel.BirthDate,
                IsSmoker = saveViewModel.IsSmoker,
                Allergies = saveViewModel.Allergies,
                ConsultoryId = consultoryId,
                PhotoUrl = saveViewModel.PhotoUrl
            };

            patient = await _patientRepository.AddAsync(patient);
            saveViewModel.Id = patient.Id;

            return saveViewModel;
        }

        public async Task UpdateAsync(PatientSaveViewModel saveViewModel)
        {
            var existingPatient = await _patientRepository.GetByIdAsync(saveViewModel.Id);

            existingPatient.FirstName = saveViewModel.Firstname;
            existingPatient.LastName = saveViewModel.Lastname;
            existingPatient.Phone = saveViewModel.Phone;
            existingPatient.Address = saveViewModel.Address;
            existingPatient.IdCard = saveViewModel.IdCard;
            existingPatient.BirthDate = saveViewModel.BirthDate;
            existingPatient.IsSmoker = saveViewModel.IsSmoker;
            existingPatient.Allergies = saveViewModel.Allergies;

            if (!string.IsNullOrWhiteSpace(saveViewModel.PhotoUrl))
                existingPatient.PhotoUrl = saveViewModel.PhotoUrl;

            await _patientRepository.UpdateAsync(existingPatient);
        }

        public async Task<List<PatientViewModel>> GetAllAsync()
        {
            var patients = await _patientRepository.GetAllAsync();
            return patients.Where(p => p.ConsultoryId == _userSessionService.GetUserInSession("assis").ConsultoryId).Select(patient => new PatientViewModel
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Phone = patient.Phone,
                Address = patient.Address,
                IdCard = patient.IdCard,
                BirthDate = patient.BirthDate,
                IsSmoker = patient.IsSmoker,
                Allergies = patient.Allergies,
                PhotoUrl = patient.PhotoUrl
            }).ToList();
        }

        public async Task<PatientSaveViewModel> GetByIdAsync(int id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);

            return new PatientSaveViewModel
            {
                Id = patient.Id,
                Firstname = patient.FirstName,
                Lastname = patient.LastName,
                Phone = patient.Phone,
                Address = patient.Address,
                IdCard = patient.IdCard,
                BirthDate = patient.BirthDate,
                IsSmoker = patient.IsSmoker,
                Allergies = patient.Allergies,
                PhotoUrl = patient.PhotoUrl
            };
        }

        public async Task RemoveAsync(int id)
        {
            try
            {
                await _patientRepository.RemoveAsync(await _patientRepository.GetByIdAsync(id));
            } 
            catch(Exception)
            {
                throw new Exception("Cannot delete this patient because has appointments or lab results.");
            }
        }
    }
}
