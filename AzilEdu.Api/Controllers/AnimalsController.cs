using AzilEdu.Api.Data;
using AzilEdu.Shared.DTOs;
using AzilEdu.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AzilEdu.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnimalsController : ControllerBase
{
    private readonly AzilEduDbContext _context;

    public AnimalsController(AzilEduDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<AnimalDto>>> GetAnimals()
    {
        var animals = await _context.Animals
            .OrderBy(a => a.Name)
            .Select(a => new AnimalDto
            {
                Id = a.Id,
                Name = a.Name,
                Species = a.Species,
                Breed = a.Breed,
                Gender = a.Gender,
                Age = a.Age,
                ArrivalDate = a.ArrivalDate,
                IsAdopted = a.IsAdopted,
                ImageUrl = a.ImageUrl,
                Description = a.Description
            })
            .ToListAsync();

        return Ok(animals);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AnimalDto>> GetAnimalById(int id)
    {
        var animal = await _context.Animals.FindAsync(id);

        if (animal is null)
            return NotFound();

        var dto = new AnimalDto
        {
            Id = animal.Id,
            Name = animal.Name,
            Species = animal.Species,
            Breed = animal.Breed,
            Gender = animal.Gender,
            Age = animal.Age,
            ArrivalDate = animal.ArrivalDate,
            IsAdopted = animal.IsAdopted,
            ImageUrl = animal.ImageUrl,
            Description = animal.Description
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<AnimalDto>> CreateAnimal(SaveAnimalDto dto)
    {
        var animal = new Animal
        {
            Name = dto.Name,
            Species = dto.Species,
            Breed = dto.Breed,
            Gender = dto.Gender,
            Age = dto.Age,
            ArrivalDate = dto.ArrivalDate,
            IsAdopted = dto.IsAdopted,
            ImageUrl = dto.ImageUrl,
            Description = dto.Description
        };

        _context.Animals.Add(animal);
        await _context.SaveChangesAsync();

        var result = new AnimalDto
        {
            Id = animal.Id,
            Name = animal.Name,
            Species = animal.Species,
            Breed = animal.Breed,
            Gender = animal.Gender,
            Age = animal.Age,
            ArrivalDate = animal.ArrivalDate,
            IsAdopted = animal.IsAdopted,
            ImageUrl = animal.ImageUrl,
            Description = animal.Description
        };

        return CreatedAtAction(nameof(GetAnimalById), new { id = animal.Id }, result);
    }
}