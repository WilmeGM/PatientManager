using Microsoft.Extensions.DependencyInjection;
using PatientManager.Core.Application.Interfaces.Services;
using PatientManager.Core.Application.Services;

namespace PatientManager.Core.Application.Settings
{
    public static class ApplicationSettings
    {
        public static void AddApplicationSettings(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IDoctorService, DoctorService>();
            services.AddTransient<ILaboratoryTestService, LaboratoryTestService>();
            services.AddTransient<ILaboratoryResultService, LaboratoryResultService>();
            services.AddTransient<IPatientService, PatientService>();
            services.AddTransient<IAppointmentService, AppointmentService>();
            services.AddTransient<IUserSessionService, UserSessionService>();
        }
    }
}
