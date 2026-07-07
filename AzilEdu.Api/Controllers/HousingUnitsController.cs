using AzilEdu.Api.Data;
using AzilEdu.Shared.DTOs;
using AzilEdu.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AzilEdu.Api.Controllers;

[ApiController]
[Route("api/housing-units")]
public class HousingUnitsController : ControllerBase
{
    private readonly AzilEduDbContext _context;

    public HousingUnitsController(AzilEduDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<HousingUnitDto>>> GetHousingUnits()
    {
        var housingUnits = await _context.HousingUnits
            .OrderBy(a => a.Name)
            .Select(a=> new HousingUnitDto
            {
                Id = a.Id,
                Name = a.Name,
                UnitType=a.UnitType,
                Capacity = a.Capacity,
                Occupied= a.Occupied,
                LastCleanedAt = a.LastCleanedAt,
                IsActive = a.IsActive,
                ImageUrl = a.ImageUrl,
                Note = a.Note
            })
            .ToListAsync();

        return Ok(housingUnits);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<HousingUnitDto>> GetHousingUnitsById(int id)
    {
        var housingUnit = await _context.HousingUnits.FindAsync(id);

        if (housingUnit is null)
            return NotFound();

        var dto = new HousingUnitDto
        {
            Id = housingUnit.Id,
            Name = housingUnit.Name,
            UnitType = housingUnit.UnitType,
            Capacity = housingUnit.Capacity,
            Occupied = housingUnit.Occupied,
            LastCleanedAt = housingUnit.LastCleanedAt,
            IsActive = housingUnit.IsActive,
            ImageUrl = housingUnit.ImageUrl,
            Note = housingUnit.Note
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<HousingUnitDto>> CreateHousingUnit(SaveHousingUnitDto dto)
    {
        var housingUnit = new HousingUnit
        {
            Name = dto.Name,
            UnitType = dto.UnitType,
            Capacity = dto.Capacity,
            Occupied = dto.Occupied,
            LastCleanedAt = dto.LastCleanedAt,
            IsActive = dto.IsActive,
            ImageUrl = dto.ImageUrl,
            Note = dto.Note
        };

        _context.HousingUnits.Add(housingUnit);
        await _context.SaveChangesAsync();

        var result = new HousingUnitDto
        {
            Name = housingUnit.Name,
            UnitType = housingUnit.UnitType,
            Capacity = housingUnit.Capacity,
            Occupied = housingUnit.Occupied,
            LastCleanedAt = housingUnit.LastCleanedAt,
            IsActive = housingUnit.IsActive,
            ImageUrl = housingUnit.ImageUrl,
            Note = housingUnit.Note
        };

        return CreatedAtAction(nameof(GetHousingUnitsById), new { id = housingUnit.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateHousingUnit(int id, SaveHousingUnitDto dto)
    {
        var housingUnit = await _context.HousingUnits.FindAsync(id);

        if (housingUnit is null)
            return NotFound();

        housingUnit.Name = dto.Name;
        housingUnit.UnitType = dto.UnitType;
        housingUnit.Capacity = dto.Capacity;
        housingUnit.Occupied = dto.Occupied;
        housingUnit.LastCleanedAt = dto.LastCleanedAt;
        housingUnit.IsActive = dto.IsActive;
        housingUnit.ImageUrl = dto.ImageUrl;
        housingUnit.Note = dto.Note;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHousingUnit(int id)
    {
        var housingUnit = await _context.HousingUnits.FindAsync(id);

        if (housingUnit is null)
            return NotFound();

        _context.HousingUnits.Remove(housingUnit);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}