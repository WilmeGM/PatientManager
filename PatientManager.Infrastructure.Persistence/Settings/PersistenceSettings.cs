using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PatientManager.Core.Application.Interfaces.Repositories;
using PatientManager.Infrastructure.Persistence.Contexts;
using PatientManager.Infrastructure.Persistence.Repositories;

namespace PatientManager.Infrastructure.Persistence.Settings
{
    public static class PersistenceSettings
    {
        public static void AddPersistenceSettings(this IServiceCollection services, IConfiguration configuration)
        {
            #region "DbContext Configuration"
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("AppDb"));
            }
            else
            {
                var connectionString = configuration.GetConnectionString("Connection");
                services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString, migrations => migrations.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }
            #endregion

            #region "Repositories DI"
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IConsultoryRepository, ConsultoryRepository>();
            services.AddTransient<IDoctorRepository, DoctorRepository>();
            services.AddTransient<ILaboratoryTestRepository, LaboratoryTestRepository>();
            services.AddTransient<IPatientRepository, PatientRepository>();
            services.AddTransient<ILaboratoryResultRepository, LaboratoryResultRepository>();
            services.AddTransient<IAppointmentRepository, AppointmentRepository>();
            #endregion
        }
    }
}
