using AzilEdu.Api.Data;
using Microsoft.EntityFrameworkCore;
using AzilEdu.Shared.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



builder.Services.AddControllers();

builder.Services.AddOpenApi();


builder.Services.AddDbContext<AzilEduDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AzilEduDbContext>();

    await db.Database.MigrateAsync();

    if (!await db.Animals.AnyAsync())
    {
        db.Animals.AddRange(
            new Animal
            {
                Name = "Luna",
                Species = "Pas",
                Breed = "Labrador",
                Gender = "Ženka",
                Age = 3,
                ArrivalDate = new DateTime(2025, 10, 12),
                IsAdopted = false,
                ImageUrl = "/images/animals/luna.webp",
                Description = "Mirna i druželjubiva kujica koja voli šetnje."
            },
            new Animal
            {
                Name = "Maza",
                Species = "Mačka",
                Breed = "Domaća kratkodlaka",
                Gender = "Ženka",
                Age = 2,
                ArrivalDate = new DateTime(2025, 11, 5),
                IsAdopted = true,
                ImageUrl = "/images/animals/maza.webp",
                Description = "Zaigrana mačka naviknuta na boravak u zatvorenom prostoru."
            },
            new Animal
            {
                Name = "Rex",
                Species = "Pas",
                Breed = "Njemački ovčar",
                Gender = "Mužjak",
                Age = 5,
                ArrivalDate = new DateTime(2026, 1, 20),
                IsAdopted = false,
                ImageUrl = "/images/animals/rex.webp",
                Description = "Aktivan pas koji traži iskusnijeg vlasnika."
            },
            new Animal
            {
                Name = "Nala",
                Species = "Mačka",
                Breed = "Maine Coon mješanac",
                Gender = "Ženka",
                Age = null,
                ArrivalDate = new DateTime(2026, 2, 3),
                IsAdopted = false,
                ImageUrl = "/images/animals/nala.webp",
                Description = "Mlada mačka pronađena bez poznate povijesti."
            },
            new Animal
            {
                Name = "Tobi",
                Species = "Pas",
                Breed = "Mješanac",
                Gender = "Mužjak",
                Age = 1,
                ArrivalDate = null,
                IsAdopted = false,
                ImageUrl = "/images/animals/tobi.webp",
                Description = "Vesel pas kojem datum dolaska još nije potvrđen."
            },
            new Animal
            {
                Name = "Bruno",
                Species = "Pas",
                Breed = "Bigl",
                Gender = "Mužjak",
                Age = 4,
                ArrivalDate = new DateTime(2025, 9, 18),
                IsAdopted = true,
                ImageUrl = "/images/animals/bruno.webp",
                Description = "Udomljen pas koji ostaje u evidenciji azila."
            }
        );


            await db.SaveChangesAsync();
    }
    if (!await db.HousingUnits.AnyAsync())
    {
        db.HousingUnits.AddRange(
        new HousingUnit
        {
            Id = 1,
            Name = "Boks A1",
            UnitType = "Boks za pse",
            Capacity = 4,
            Occupied = 4,
            LastCleanedAt = DateTime.Now.AddDays(-1),
            IsActive = true,
            ImageUrl = "/images/units/box-1.webp",
            Note = "Veliki boks za pse"
        },

        new HousingUnit
        {
            Id = 2,
            Name = "Boks B2",
            UnitType = "Boks za pse",
            Capacity = 3,
            Occupied = 1,
            LastCleanedAt = DateTime.Now.AddDays(-3),
            IsActive = true,
            ImageUrl = "/images/units/box-2.jpg",
            Note = "Mirniji smještaj za manjeg psa"
        },

        new HousingUnit
        {
            Id = 3,
            Name = "Mačji prostor M1",
            UnitType = "Prostor za mačke",
            Capacity = 6,
            Occupied = 3,
            LastCleanedAt = DateTime.Now.AddDays(-2),
            IsActive = true,
            ImageUrl = "/images/units/cat-room.jpg",
            Note = "Odvojeni prostor s penjalicama"
        },

        new HousingUnit
        {
            Id = 4,
            Name = "Karantena K1",
            UnitType = "Karantena",
            Capacity = 2,
            Occupied = 1,
            LastCleanedAt = null,
            IsActive = true,
            ImageUrl = "/images/units/quarantine.webp",
            Note = "Čeka dezinfekciju"
        },

        new HousingUnit
        {
            Id = 5,
            Name = "Privremeni smještaj P1",
            UnitType = "Privremeni smještaj",
            Capacity = 5,
            Occupied = 0,
            LastCleanedAt = DateTime.Now.AddDays(-7),
            IsActive = false,
            ImageUrl = "/images/units/inactive-unit.webp",
            Note = "Trenutno izvan uporabe"
        },

        new HousingUnit
        {
            Id = 6,
            Name = "Boks C3",
            UnitType = "Boks za pse",
            Capacity = 2,
            Occupied = 2,
            LastCleanedAt = DateTime.Now.AddDays(-5),
            IsActive = true,
            ImageUrl = "/images/units/yard-unit.jpg",
            Note = "Mali boks za dva psa"
        }

        );
        await db.SaveChangesAsync();
    }
}

app.Run();
