using DepiProject.Models;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace DepiProject.DbContextLayer
{
    public class HospitalContext : DbContext
    {
        public HospitalContext(DbContextOptions<HospitalContext> options)
            : base(options)
        {
        }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Nurse> Nurses { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<InsuranceProvider> InsuranceProviders { get; set; }
        public DbSet<Billing> Billings { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed Departments
            modelBuilder.Entity<Department>().HasData(
                new Department { ID = 1, Name = "Cardiology" },
                new Department { ID = 2, Name = "Neurology" }
            );

            // Seed Doctors
            modelBuilder.Entity<Doctor>().HasData(
                new Doctor { ID = 1, Name = "Dr. John Smith", ContactInfo = "123-456-7890", Specialty = "Cardiologist", DepartmentID = 1 },
                new Doctor { ID = 2, Name = "Dr. Alice Johnson", ContactInfo = "987-654-3210", Specialty = "Neurologist", DepartmentID = 2 }
            );

            // Seed Nurses
            modelBuilder.Entity<Nurse>().HasData(
                new Nurse { ID = 1, Name = "Nurse Mary", ContactInfo = "555-123-4567", DepartmentID = 1 },
                new Nurse { ID = 2, Name = "Nurse Susan", ContactInfo = "555-987-6543", DepartmentID = 2 }
            );

            // Seed Insurance Providers
            modelBuilder.Entity<InsuranceProvider>().HasData(
                new InsuranceProvider { ID = 1, CompanyName = "HealthCare Inc.", ContactInfo = "555-111-2222" },
                new InsuranceProvider { ID = 2, CompanyName = "Wellness Ltd.", ContactInfo = "555-333-4444" }
            );

            modelBuilder.Entity<Patient>().HasData(
        new Patient { ID = 1, Name = "Patient A", ContactInfo = "555-555-5555", InsuranceProviderId = 1},
        new Patient { ID = 2, Name = "Patient B", ContactInfo = "555-666-6666", InsuranceProviderId = 2 }
    );

            // Seed Services
            modelBuilder.Entity<Service>().HasData(
                new Service { ID = 1, Name = "Routine Checkup", Cost = 100.00m },
                new Service { ID = 2, Name = "Specialist Consultation", Cost = 200.00m }
            );

            // Seed Appointments
            modelBuilder.Entity<Appointment>().HasData(
                new Appointment { ID = 1, Date = DateTime.Now.AddDays(1), Status = "Scheduled", PatientID = 1, DoctorID = 1, NurseID = 1 },
                new Appointment { ID = 2, Date = DateTime.Now.AddDays(2), Status = "Scheduled", PatientID = 2, DoctorID = 2, NurseID = 2 }
            );

            // Seed Medical Records
            modelBuilder.Entity<MedicalRecord>().HasData(
                new MedicalRecord { ID = 1, Date = DateTime.Now.AddDays(-30), Notes = "Initial consultation", PatientID = 1, DoctorID = 1 },
                new MedicalRecord { ID = 2, Date = DateTime.Now.AddDays(-15), Notes = "Follow-up visit", PatientID = 2, DoctorID = 2 }
            );

            // Seed Billings
            modelBuilder.Entity<Billing>().HasData(
                new Billing { ID = 1, Amount = 100.00m, PayMethod = "Credit Card", PatientID = 1 },
                new Billing { ID = 2, Amount = 200.00m, PayMethod = "Cash", PatientID = 2 }
            );

            // Seed Medications
            modelBuilder.Entity<Medication>().HasData(
                new Medication { ID = 1, Name = "Medication A", Dosage = "2 tablets daily", PatientID = 1 },
                new Medication { ID = 2, Name = "Medication B", Dosage = "1 tablet daily", PatientID = 2 }
            );

            // Seed Feedbacks
            modelBuilder.Entity<Feedback>().HasData(
                new Feedback { ID = 1, Rating = 5, Comment = "Excellent care", PatientID = 1, DoctorID = 1 },
                new Feedback { ID = 2, Rating = 4, Comment = "Very good service", PatientID = 2, DoctorID = 2 }
            );
        modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Nurse)
                .WithMany(n => n.Appointments)
                .HasForeignKey(a => a.NurseID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Department)
                .WithMany(dep => dep.Doctors)
                .HasForeignKey(d => d.DepartmentID);

            modelBuilder.Entity<Nurse>()
                .HasOne(n => n.Department)
                .WithMany(dep => dep.Nurses)
                .HasForeignKey(n => n.DepartmentID);

            modelBuilder.Entity<Service>()
                .HasMany(s => s.Departments)
                .WithMany(d => d.Services);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Patient)
                .WithMany(p => p.Feedbacks)
                .HasForeignKey(f => f.PatientID);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Doctor)
                .WithMany(d => d.Feedbacks)
                .HasForeignKey(f => f.DoctorID);

            modelBuilder.Entity<MedicalRecord>()
                .HasOne(m => m.Patient)
                .WithMany(p => p.MedicalRecords)
                .HasForeignKey(m => m.PatientID);

            modelBuilder.Entity<MedicalRecord>()
                .HasOne(m => m.Doctor)
                .WithMany(d => d.MedicalRecords)
                .HasForeignKey(m => m.DoctorID);

            modelBuilder.Entity<Billing>()
                .HasOne(b => b.Patient)
                .WithMany(p => p.Billings)
                .HasForeignKey(b => b.PatientID);

            modelBuilder.Entity<Medication>()
                .HasOne(m => m.Patient)
                .WithMany(p => p.Medications)
                .HasForeignKey(m => m.PatientID);

        }
    }

}
