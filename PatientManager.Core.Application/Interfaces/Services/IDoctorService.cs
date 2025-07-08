using PatientManager.Core.Application.ViewModels.Doctors;

namespace PatientManager.Core.Application.Interfaces.Services
{
    public interface IDoctorService : IGenericService<DoctorSaveViewModel, DoctorViewModel>, IUpdateService<DoctorSaveViewModel>
    {

    }
}
