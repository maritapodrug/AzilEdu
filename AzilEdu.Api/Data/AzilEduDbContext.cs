using AzilEdu.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace AzilEdu.Api.Data
{
    public class AzilEduDbContext : DbContext
    {
        public AzilEduDbContext(DbContextOptions<AzilEduDbContext> options)
        : base(options)
        {
        }

        public DbSet<Animal> Animals => Set<Animal>();
        public DbSet<HousingUnit> HousingUnits => Set<HousingUnit>();
        public DbSet<AnimalStatus> AnimalStatuses => Set<AnimalStatus>();

        public DbSet<Volunteer> Volunteers => Set<Volunteer>();
        public DbSet<VolunteerStatus> VolunteerStatuses => Set<VolunteerStatus>();
        public DbSet<VolunteerTask> VolunteerTasks => Set<VolunteerTask>();
        public DbSet<VolunteerTaskStatus> VolunteerTaskStatuses => Set<VolunteerTaskStatus>();
        public DbSet<VolunteerTaskType> VolunteerTaskTypes => Set<VolunteerTaskType>();
        
        public DbSet<Donor> Donors => Set<Donor>();
        public DbSet<DonorType> DonorTypes => Set<DonorType>();
        public DbSet<DonorStatus> DonorStatuses => Set<DonorStatus>();

        public DbSet<Donation> Donations => Set<Donation>();
        public DbSet<DonationType> DonationTypes => Set<DonationType>();
        public DbSet<DonationStatus> DonationStatuses => Set<DonationStatus>();

        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<EmployeePosition> EmployeePositions => Set<EmployeePosition>();
        public DbSet<EmployeeStatus> EmployeeStatuses => Set<EmployeeStatus>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Animal -> AnimalStatus
            modelBuilder.Entity<Animal>()
                .HasOne(animal => animal.AnimalStatus)
                .WithMany(status => status.Animals)
                .HasForeignKey(animal => animal.AnimalStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AnimalStatus>().HasData(
                new AnimalStatus { Id = 1, Name = "Dostupna za udomljenje" },
                new AnimalStatus { Id = 2, Name = "Rezervirana" },
                new AnimalStatus { Id = 3, Name = "Udomljena" },
                new AnimalStatus { Id = 4, Name = "Na liječenju" }
            );

            // Volunteer -> VolunteerStatus
            modelBuilder.Entity<Volunteer>()
                .HasOne(v => v.VolunteerStatus)
                .WithMany(s => s.Volunteers)
                .HasForeignKey(v => v.VolunteerStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VolunteerStatus>().HasData(
                new VolunteerStatus { Id = 1, Name = "Novi" },
                new VolunteerStatus { Id = 2, Name = "Aktivan" },
                new VolunteerStatus { Id = 3, Name = "Privremeno nedostupan" },
                new VolunteerStatus { Id = 4, Name = "Neaktivan" }
            );

            // Donor -> DonorType
            modelBuilder.Entity<Donor>()
                .HasOne(d => d.DonorType)
                .WithMany(t => t.Donors)
                .HasForeignKey(d => d.DonorTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Donor -> DonorStatus
            modelBuilder.Entity<Donor>()
                .HasOne(d => d.DonorStatus)
                .WithMany(s => s.Donors)
                .HasForeignKey(d => d.DonorStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DonorType>().HasData(
                new DonorType { Id = 1, Name = "Fizička osoba" },
                new DonorType { Id = 2, Name = "Tvrtka" },
                new DonorType { Id = 3, Name = "Udruga ili organizacija" }
            );

            modelBuilder.Entity<DonorStatus>().HasData(
                new DonorStatus { Id = 1, Name = "Novi" },
                new DonorStatus { Id = 2, Name = "Aktivan" },
                new DonorStatus { Id = 3, Name = "Povremeni" },
                new DonorStatus { Id = 4, Name = "Neaktivan" }
            );

            // Employee -> EmployeePosition
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.EmployeePosition)
                .WithMany(p => p.Employees)
                .HasForeignKey(e => e.EmployeePositionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Employee -> EmployeeStatus
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.EmployeeStatus)
                .WithMany(s => s.Employees)
                .HasForeignKey(e => e.EmployeeStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EmployeePosition>().HasData(
                new EmployeePosition { Id = 1, Name = "Djelatnik azila" },
                new EmployeePosition { Id = 2, Name = "Veterinar" },
                new EmployeePosition { Id = 3, Name = "Koordinator volontera" },
                new EmployeePosition { Id = 4, Name = "Administrator" }
            );

            modelBuilder.Entity<EmployeeStatus>().HasData(
                new EmployeeStatus { Id = 1, Name = "Aktivan" },
                new EmployeeStatus { Id = 2, Name = "Na dopustu ili bolovanju" },
                new EmployeeStatus { Id = 3, Name = "Neaktivan" }
            );

            // --- Dummy podaci: Volunteers ---
            modelBuilder.Entity<Volunteer>().HasData(
                new Volunteer { Id = 1, FirstName = "Ana", LastName = "Horvat", Email = "ana.horvat@email.com", Phone = "091 234 5678", Skills = "Briga o psima, šetanje", AvailableFrom = new DateTime(2025, 1, 15), Notes = "Dostupna vikendom", VolunteerStatusId = 2 },
                new Volunteer { Id = 2, FirstName = "Marko", LastName = "Perić", Email = "marko.peric@email.com", Phone = "098 765 4321", Skills = "Fotografija životinja, društvene mreže", AvailableFrom = new DateTime(2025, 3, 1), Notes = "Fotograf volonter", VolunteerStatusId = 2 },
                new Volunteer { Id = 3, FirstName = "Ivana", LastName = "Kovač", Email = "ivana.kovac@email.com", Phone = "095 111 2233", Skills = "Veterinarski tehničar", AvailableFrom = new DateTime(2024, 9, 10), Notes = "Može pomagati pri veterinarskim pregledima", VolunteerStatusId = 2 },
                new Volunteer { Id = 4, FirstName = "Tomislav", LastName = "Babić", Email = "tomislav.babic@email.com", Phone = "092 444 5566", Skills = "Edukacija životinja, dresura", AvailableFrom = null, Notes = "Privremeno nedostupan zbog posla", VolunteerStatusId = 3 },
                new Volunteer { Id = 5, FirstName = "Petra", LastName = "Novak", Email = "petra.novak@email.com", Phone = "099 888 7766", Skills = "Administracija, pisanje molbi", AvailableFrom = new DateTime(2026, 2, 1), Notes = "Novi volonter, u uvođenju", VolunteerStatusId = 1 },
                new Volunteer { Id = 6, FirstName = "Luka", LastName = "Marić", Email = "luka.maric@email.com", Phone = "091 321 6549", Skills = "Transport životinja, vozač", AvailableFrom = new DateTime(2024, 6, 20), Notes = "Može prevoziti životinje na veterinara", VolunteerStatusId = 2 },
                new Volunteer { Id = 7, FirstName = "Maja", LastName = "Jurić", Email = "maja.juric@email.com", Phone = "098 654 3210", Skills = "Šivanje, izrada igračaka za životinje", AvailableFrom = null, Notes = "Završila volontiranje", VolunteerStatusId = 4 }
            );

            // --- Dummy podaci: Donors ---
            modelBuilder.Entity<Donor>().HasData(
                new Donor { Id = 1, FirstName = "Stjepan", LastName = "Blažević", OrganizationName = "", Email = "stjepan.blazevic@gmail.com", Phone = "091 555 1234", Address = "Ilica 42", City = "Zagreb", Notes = "Redoviti donator hrane", CreatedAt = new DateTime(2024, 3, 10), DonorTypeId = 1, DonorStatusId = 2 },
                new Donor { Id = 2, FirstName = "", LastName = "", OrganizationName = "Zooplus Hrvatska d.o.o.", Email = "donacije@zooplus.hr", Phone = "01 234 5678", Address = "Avenija Dubrovnik 15", City = "Zagreb", Notes = "Donira hranu i opremu kvartalno", CreatedAt = new DateTime(2023, 11, 5), DonorTypeId = 2, DonorStatusId = 2 },
                new Donor { Id = 3, FirstName = "Karin", LastName = "Šimić", OrganizationName = "", Email = "karin.simic@outlook.com", Phone = "095 999 4455", Address = "Vukovarska 88", City = "Split", Notes = "Donira povremeno, kontaktirati pred kampanje", CreatedAt = new DateTime(2025, 1, 22), DonorTypeId = 1, DonorStatusId = 3 },
                new Donor { Id = 4, FirstName = "", LastName = "", OrganizationName = "Udruga Prijatelji životinja", Email = "info@prijatelji-zivotinja.hr", Phone = "01 987 6543", Address = "Tratinska 10", City = "Zagreb", Notes = "Suradnja na edukacijskim projektima", CreatedAt = new DateTime(2024, 7, 18), DonorTypeId = 3, DonorStatusId = 2 },
                new Donor { Id = 5, FirstName = "Boris", LastName = "Kralj", OrganizationName = "", Email = "boris.kralj@gmail.com", Phone = "092 111 3344", Address = "Maksimirska 60", City = "Zagreb", Notes = "Novi donator, uplatio prvu donaciju", CreatedAt = new DateTime(2026, 4, 3), DonorTypeId = 1, DonorStatusId = 1 },
                new Donor { Id = 6, FirstName = "", LastName = "", OrganizationName = "Petcenter d.o.o.", Email = "marketing@petcenter.hr", Phone = "052 345 6789", Address = "Riva 22", City = "Pula", Notes = "Donira opremu i igračke", CreatedAt = new DateTime(2024, 2, 14), DonorTypeId = 2, DonorStatusId = 2 },
                new Donor { Id = 7, FirstName = "Nikolina", LastName = "Đukić", OrganizationName = "", Email = "nikolina.djukic@yahoo.com", Phone = "098 222 7788", Address = "Korzo 5", City = "Rijeka", Notes = "Prestala donirati 2024.", CreatedAt = new DateTime(2022, 8, 30), DonorTypeId = 1, DonorStatusId = 4 }
            );

            // --- Dummy podaci: Employees ---
            modelBuilder.Entity<Employee>().HasData(
                new Employee { Id = 1, FirstName = "Renata", LastName = "Štefanec", Email = "renata.stefanec@azil.hr", Phone = "091 700 1001", EmployeeNumber = "AZ-001", HireDate = new DateTime(2019, 4, 1), Notes = "Odgovorna za smještaj i ishranu životinja", EmployeePositionId = 1, EmployeeStatusId = 1 },
                new Employee { Id = 2, FirstName = "Dr. Ivan", LastName = "Posavec", Email = "ivan.posavec@azil.hr", Phone = "091 700 1002", EmployeeNumber = "AZ-002", HireDate = new DateTime(2020, 9, 15), Notes = "Stalni veterinar, radi pon-pet", EmployeePositionId = 2, EmployeeStatusId = 1 },
                new Employee { Id = 3, FirstName = "Sandra", LastName = "Filipović", Email = "sandra.filipovic@azil.hr", Phone = "091 700 1003", EmployeeNumber = "AZ-003", HireDate = new DateTime(2021, 2, 10), Notes = "Koordinira raspored volontera", EmployeePositionId = 3, EmployeeStatusId = 1 },
                new Employee { Id = 4, FirstName = "Goran", LastName = "Tkalčić", Email = "goran.tkalcic@azil.hr", Phone = "091 700 1004", EmployeeNumber = "AZ-004", HireDate = new DateTime(2018, 6, 1), Notes = "Vodi administraciju i financije", EmployeePositionId = 4, EmployeeStatusId = 1 },
                new Employee { Id = 5, FirstName = "Mirela", LastName = "Vuković", Email = "mirela.vukovic@azil.hr", Phone = "091 700 1005", EmployeeNumber = "AZ-005", HireDate = new DateTime(2022, 11, 7), Notes = "Brine o mačkama i malim životinjama", EmployeePositionId = 1, EmployeeStatusId = 2 },
                new Employee { Id = 6, FirstName = "Davor", LastName = "Knežević", Email = "davor.knezevic@azil.hr", Phone = "091 700 1006", EmployeeNumber = "AZ-006", HireDate = new DateTime(2023, 3, 20), Notes = "Zamjenski djelatnik, radi na terenu", EmployeePositionId = 1, EmployeeStatusId = 1 }
            );

            modelBuilder.Entity<VolunteerTask>()
                .HasOne(task => task.Volunteer)
                .WithMany()
                .HasForeignKey(task => task.VolunteerId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<VolunteerTask>()
                .HasOne(task => task.Animal)
                .WithMany()
                .HasForeignKey(task => task.AnimalId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<VolunteerTask>()
                .HasOne(task => task.VolunteerTaskStatus)
                .WithMany(status => status.Tasks)
                .HasForeignKey(task => task.VolunteerTaskStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VolunteerTask>()
                .HasOne(task => task.VolunteerTaskType)
                .WithMany(type => type.Tasks)
                .HasForeignKey(task => task.VolunteerTaskTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VolunteerTaskStatus>().HasData(
                new VolunteerTaskStatus { Id = 1, Name = "Otvoren" },
                new VolunteerTaskStatus { Id = 2, Name = "Dodijeljen" },
                new VolunteerTaskStatus { Id = 3, Name = "U tijeku" },
                new VolunteerTaskStatus { Id = 4, Name = "Završeno" },
                new VolunteerTaskStatus { Id = 5, Name = "Otkazano" }
            );

            modelBuilder.Entity<VolunteerTaskType>().HasData(
                new VolunteerTaskType { Id = 1, Name = "Šetnja" },
                new VolunteerTaskType { Id = 2, Name = "Hranjenje" },
                new VolunteerTaskType { Id = 3, Name = "Čišćenje" },
                new VolunteerTaskType { Id = 4, Name = "Socijalizacija" },
                new VolunteerTaskType { Id = 5, Name = "Prijevoz" },
                new VolunteerTaskType { Id = 6, Name = "Administracija" }
            );

            // Donation -> Donor
            modelBuilder.Entity<Donation>()
                .HasOne(d => d.Donor)
                .WithMany()
                .HasForeignKey(d => d.DonorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Donation -> DonationType
            modelBuilder.Entity<Donation>()
                .HasOne(d => d.DonationType)
                .WithMany(t => t.Donations)
                .HasForeignKey(d => d.DonationTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Donation -> DonationStatus
            modelBuilder.Entity<Donation>()
                .HasOne(d => d.DonationStatus)
                .WithMany(s => s.Donations)
                .HasForeignKey(d => d.DonationStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DonationType>().HasData(
                new DonationType { Id = 1, Name = "Novčana" },
                new DonationType { Id = 2, Name = "Hrana" },
                new DonationType { Id = 3, Name = "Oprema" },
                new DonationType { Id = 4, Name = "Lijekovi" },
                new DonationType { Id = 5, Name = "Usluga" }
            );

            modelBuilder.Entity<DonationStatus>().HasData(
                new DonationStatus { Id = 1, Name = "Evidentirana" },
                new DonationStatus { Id = 2, Name = "Potvrđena" },
                new DonationStatus { Id = 3, Name = "Iskorištena" },
                new DonationStatus { Id = 4, Name = "Otkazana" }
            );

            // --- Dummy podaci: Donations ---
            modelBuilder.Entity<Donation>().HasData(
                new Donation { Id = 1, DonorId = 1, DonationTypeId = 2, DonationStatusId = 3, DonationDate = new DateTime(2024, 4, 5), ItemName = "Suha hrana za pse", Quantity = 50, EstimatedValue = 750, Notes = "Donirao 50 kg hrane" },
                new Donation { Id = 2, DonorId = 2, DonationTypeId = 3, DonationStatusId = 2, DonationDate = new DateTime(2024, 6, 12), ItemName = "Kavezi i povodci", Quantity = 10, EstimatedValue = 1500, Notes = "Oprema za prihvat novih životinja" },
                new Donation { Id = 3, DonorId = 1, DonationTypeId = 1, DonationStatusId = 2, DonationDate = new DateTime(2024, 9, 1), Amount = 500, Notes = "Novčana donacija za veterinara" },
                new Donation { Id = 4, DonorId = 4, DonationTypeId = 5, DonationStatusId = 2, DonationDate = new DateTime(2024, 10, 20), ItemName = "Edukacija volontera", EstimatedValue = 800, Notes = "Udruga organizirala edukaciju" },
                new Donation { Id = 5, DonorId = 3, DonationTypeId = 2, DonationStatusId = 1, DonationDate = new DateTime(2025, 2, 14), ItemName = "Mokra hrana za mačke", Quantity = 100, EstimatedValue = 400, Notes = "Doneseno osobno" },
                new Donation { Id = 6, DonorId = 6, DonationTypeId = 3, DonationStatusId = 2, DonationDate = new DateTime(2025, 4, 3), ItemName = "Igračke i posteljine", Quantity = 20, EstimatedValue = 600, Notes = "Sezonska donacija opreme" },
                new Donation { Id = 7, DonorId = 5, DonationTypeId = 1, DonationStatusId = 1, DonationDate = new DateTime(2026, 4, 5), Amount = 200, Notes = "Prva donacija novog donatora" }
            );
        }
    }
}
