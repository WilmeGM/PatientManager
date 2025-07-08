using PatientManager.Core.Application.ViewModels.Patients;

namespace PatientManager.Core.Application.Interfaces.Services
{
    public interface IPatientService : IGenericService<PatientSaveViewModel, PatientViewModel>, IUpdateService<PatientSaveViewModel>
    {
    }
}
