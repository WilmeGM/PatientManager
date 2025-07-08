using PatientManager.Core.Application.Interfaces.Repositories;
using PatientManager.Core.Application.Interfaces.Services;
using PatientManager.Core.Application.ViewModels.Doctors;
using PatientManager.Core.Domain.Entities;

namespace PatientManager.Core.Application.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IUserSessionService _userSessionService;

        public DoctorService(IDoctorRepository doctorRepository, IUserSessionService userSessionService)
        {
            _doctorRepository = doctorRepository;
            _userSessionService = userSessionService;
        }

        public async Task<DoctorSaveViewModel> AddAsync(DoctorSaveViewModel saveViewModel, int consultoryId)
        {
            var doctor = new Doctor
            {
                FirstName = saveViewModel.Firstname,
                LastName = saveViewModel.Lastname,
                Email = saveViewModel.Email,
                Phone = saveViewModel.Phone,
                IdCard = saveViewModel.IdCard,
                PhotoUrl = saveViewModel.PhotoUrl,
                ConsultoryId = consultoryId
            };

            doctor = await _doctorRepository.AddAsync(doctor);

            saveViewModel.Id = doctor.Id;
            return saveViewModel;
        }

        public async Task<DoctorSaveViewModel> GetByIdAsync(int id)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);

            return new DoctorSaveViewModel
            {
                Id = doctor.Id,
                Firstname = doctor.FirstName,
                Lastname = doctor.LastName,
                Email = doctor.Email,
                Phone = doctor.Phone,
                IdCard = doctor.IdCard,
                PhotoUrl = doctor.PhotoUrl
            };
        }

        public async Task<List<DoctorViewModel>> GetAllAsync()
        {
            var doctorsList = await _doctorRepository.GetAllAsync();

            return doctorsList
                .Where(d => d.ConsultoryId == _userSessionService.GetUserInSession("admin").ConsultoryId)
                .Select(doctor => new DoctorViewModel
                {
                    Id = doctor.Id,
                    FirstName = doctor.FirstName,
                    LastName = doctor.LastName,
                    Email = doctor.Email,
                    Phone = doctor.Phone,
                    IdCard = doctor.IdCard,
                    PhotoUrl = doctor.PhotoUrl
                })
                .ToList();
        }

        public async Task RemoveAsync(int id)
        {
            try
            {
                await _doctorRepository.RemoveAsync(await _doctorRepository.GetByIdAsync(id));
            } 
            catch (Exception)
            {
                throw new Exception("Cannon delete this doctor because has appointments.");
            }
        }

        public async Task UpdateAsync(DoctorSaveViewModel saveViewModel)
        {
            var existingDoctor = await _doctorRepository.GetByIdAsync(saveViewModel.Id);

            if (existingDoctor != null)
            {
                existingDoctor.FirstName = saveViewModel.Firstname;
                existingDoctor.LastName = saveViewModel.Lastname;
                existingDoctor.Email = saveViewModel.Email;
                existingDoctor.Phone = saveViewModel.Phone;
                existingDoctor.IdCard = saveViewModel.IdCard;
                if (!string.IsNullOrEmpty(saveViewModel.PhotoUrl))
                    existingDoctor.PhotoUrl = saveViewModel.PhotoUrl;

                await _doctorRepository.UpdateAsync(existingDoctor);
            }
        }
    }
}
