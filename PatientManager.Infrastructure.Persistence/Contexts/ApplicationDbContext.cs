using Microsoft.EntityFrameworkCore;
using PatientManager.Core.Domain.Entities;

namespace PatientManager.Infrastructure.Persistence.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Consultory> Consultories { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<LaboratoryTest> LaboratoryTests { get; set; }
        public DbSet<LaboratoryResult> LaboratoryResults { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Tables
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Consultory>().ToTable("Consultories");
            modelBuilder.Entity<Doctor>().ToTable("Doctors");
            modelBuilder.Entity<Patient>().ToTable("Patients");
            modelBuilder.Entity<LaboratoryTest>().ToTable("LaboratoryTests");
            modelBuilder.Entity<LaboratoryResult>().ToTable("LaboratoryResults");
            modelBuilder.Entity<Appointment>().ToTable("Appointments");
            #endregion

            #region Primary Keys
            modelBuilder.Entity<User>().HasKey(user => user.Id);
            modelBuilder.Entity<Consultory>().HasKey(consultory => consultory.Id);
            modelBuilder.Entity<Doctor>().HasKey(doctor => doctor.Id);
            modelBuilder.Entity<Patient>().HasKey(patient => patient.Id);
            modelBuilder.Entity<LaboratoryTest>().HasKey(test => test.Id);
            modelBuilder.Entity<LaboratoryResult>().HasKey(result => result.Id);
            modelBuilder.Entity<Appointment>().HasKey(appointment => appointment.Id);
            #endregion

            #region Relationships

            // Consultory - User (1:N)
            modelBuilder.Entity<Consultory>()
                .HasMany(consultory => consultory.Users)
                .WithOne(user => user.Consultory)
                .HasForeignKey(user => user.ConsultoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Consultory - Doctor (1:N)
            modelBuilder.Entity<Consultory>()
                .HasMany(consultory => consultory.Doctors)
                .WithOne(doctor => doctor.Consultory)
                .HasForeignKey(doctor => doctor.ConsultoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Consultory - Patient (1:N)
            modelBuilder.Entity<Consultory>()
                .HasMany(consultory => consultory.Patients)
                .WithOne(patient => patient.Consultory)
                .HasForeignKey(patient => patient.ConsultoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Consultory - LaboratoryTest (1:N)
            modelBuilder.Entity<Consultory>()
                .HasMany(consultory => consultory.LaboratoryTests)
                .WithOne(test => test.Consultory)
                .HasForeignKey(test => test.ConsultoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Consultory - LaboratoryResult (1:N)
            modelBuilder.Entity<Consultory>()
                .HasMany(consultory => consultory.LaboratoryResults)
                .WithOne(result => result.Consultory)
                .HasForeignKey(result => result.ConsultoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Consultory - Appointment (1:N)
            modelBuilder.Entity<Consultory>()
                .HasMany(consultory => consultory.Appointments)
                .WithOne(appointment => appointment.Consultory)
                .HasForeignKey(appointment => appointment.ConsultoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Patient - Appointment (1:N)
            modelBuilder.Entity<Patient>()
                .HasMany(patient => patient.Appointments)
                .WithOne(appointment => appointment.Patient)
                .HasForeignKey(appointment => appointment.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Doctor - Appointment (1:N)
            modelBuilder.Entity<Doctor>()
                .HasMany(doctor => doctor.Appointments)
                .WithOne(appointment => appointment.Doctor)
                .HasForeignKey(appointment => appointment.DoctorId)
                .OnDelete(DeleteBehavior.Restrict); 

            // LaboratoryTest - LaboratoryResult (1:N)
            modelBuilder.Entity<LaboratoryTest>()
                .HasMany(test => test.LaboratoryResults)
                .WithOne(result => result.LaboratoryTest)
                .HasForeignKey(result => result.LaboratoryTestId)
                .OnDelete(DeleteBehavior.Cascade);

            //Appointment - LaboratoryResult (1:N)
            modelBuilder.Entity<Appointment>()
                .HasMany(a => a.LaboratoryResults)
                .WithOne(lr => lr.Appointment)
                .HasForeignKey(lr => lr.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Patient - LaboratoryResult (1:N)
            modelBuilder.Entity<Patient>()
                .HasMany(patient => patient.LaboratoryResults)
                .WithOne(result => result.Patient)
                .HasForeignKey(result => result.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region Property Configurations

            // User
            modelBuilder.Entity<User>()
                .Property(user => user.FirstName)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(user => user.LastName)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(user => user.Email)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(user => user.Username)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(user => user.Password)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(user => user.UserType)
                .IsRequired();

            // Consultory
            modelBuilder.Entity<Consultory>().HasIndex(c => c.Name).IsUnique();

            modelBuilder.Entity<Consultory>()
                .Property(consultory => consultory.Name)
                .IsRequired();

            // Doctor
            modelBuilder.Entity<Doctor>()
                .Property(doctor => doctor.FirstName)
                .IsRequired();

            modelBuilder.Entity<Doctor>()
                .Property(doctor => doctor.LastName)
                .IsRequired();

            modelBuilder.Entity<Doctor>()
                .Property(doctor => doctor.Email)
                .IsRequired();

            modelBuilder.Entity<Doctor>()
                .Property(doctor => doctor.Phone)
                .IsRequired();

            modelBuilder.Entity<Doctor>()
                .Property(doctor => doctor.IdCard)
                .IsRequired();

            // Patient
            modelBuilder.Entity<Patient>()
                .Property(patient => patient.FirstName)
                .IsRequired();

            modelBuilder.Entity<Patient>()
                .Property(patient => patient.LastName)
                .IsRequired();

            modelBuilder.Entity<Patient>()
                .Property(patient => patient.Phone)
                .IsRequired();

            modelBuilder.Entity<Patient>()
                .Property(patient => patient.Address)
                .IsRequired();

            modelBuilder.Entity<Patient>()
                .Property(patient => patient.IdCard)
                .IsRequired();

            modelBuilder.Entity<Patient>()
                .Property(patient => patient.BirthDate)
                .IsRequired();

            modelBuilder.Entity<Patient>()
                .Property(patient => patient.IsSmoker)
                .IsRequired();

            modelBuilder.Entity<Patient>()
                .Property(patient => patient.Allergies)
                .IsRequired();

            // LaboratoryTest
            modelBuilder.Entity<LaboratoryTest>()
                .HasIndex(test => test.Name)
                .IsUnique();

            modelBuilder.Entity<LaboratoryTest>()
                .Property(test => test.Name)
                .IsRequired();

            // LaboratoryResult
            
             
            // Appointment
            modelBuilder.Entity<Appointment>()
                .Property(appointment => appointment.Date)
                .IsRequired();

            modelBuilder.Entity<Appointment>()
                .Property(appointment => appointment.Time)
                .IsRequired();

            modelBuilder.Entity<Appointment>()
                .Property(appointment => appointment.Reason)
                .IsRequired();

            modelBuilder.Entity<Appointment>()
               .Property(appointment => appointment.Status)
               .IsRequired();

            #endregion
        }
    }
}
