using AzilEdu.Api.Data;
using AzilEdu.Shared.DTOs;
using AzilEdu.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AzilEdu.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DonationsController : ControllerBase
{
    private readonly AzilEduDbContext _context;

    public DonationsController(AzilEduDbContext context)
    {
        _context = context;
    }

    // Kasnije će donator vidjeti samo svoje donacije.
    [HttpGet]
    public async Task<ActionResult<List<DonationDto>>> GetDonations(
        [FromQuery] int? donorId,
        [FromQuery] int? typeId,
        [FromQuery] int? statusId,
        [FromQuery] DateTime? dateFrom,
        [FromQuery] DateTime? dateTo)
    {
        var query = _context.Donations
            .Include(d => d.Donor)
            .Include(d => d.DonationType)
            .Include(d => d.DonationStatus)
            .AsQueryable();

        if (donorId.HasValue)
            query = query.Where(d => d.DonorId == donorId.Value);

        if (typeId.HasValue)
            query = query.Where(d => d.DonationTypeId == typeId.Value);

        if (statusId.HasValue)
            query = query.Where(d => d.DonationStatusId == statusId.Value);

        if (dateFrom.HasValue)
            query = query.Where(d => d.DonationDate >= dateFrom.Value);

        if (dateTo.HasValue)
            query = query.Where(d => d.DonationDate <= dateTo.Value);

        var donations = await query
            .OrderByDescending(d => d.DonationDate)
            .Select(d => new DonationDto
            {
                Id = d.Id,
                DonationDate = d.DonationDate,
                Amount = d.Amount,
                ItemName = d.ItemName,
                Quantity = d.Quantity,
                EstimatedValue = d.EstimatedValue,
                Notes = d.Notes,
                DonorId = d.DonorId,
                DonorName = d.Donor != null
                    ? (string.IsNullOrWhiteSpace(d.Donor.OrganizationName)
                        ? d.Donor.FirstName + " " + d.Donor.LastName
                        : d.Donor.OrganizationName)
                    : string.Empty,
                DonationTypeId = d.DonationTypeId,
                DonationType = d.DonationType != null ? d.DonationType.Name : string.Empty,
                DonationStatusId = d.DonationStatusId,
                Status = d.DonationStatus != null ? d.DonationStatus.Name : string.Empty
            })
            .ToListAsync();

        return Ok(donations);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DonationDto>> GetDonationById(int id)
    {
        var d = await _context.Donations
            .Include(x => x.Donor)
            .Include(x => x.DonationType)
            .Include(x => x.DonationStatus)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (d is null)
            return NotFound();

        return Ok(new DonationDto
        {
            Id = d.Id,
            DonationDate = d.DonationDate,
            Amount = d.Amount,
            ItemName = d.ItemName,
            Quantity = d.Quantity,
            EstimatedValue = d.EstimatedValue,
            Notes = d.Notes,
            DonorId = d.DonorId,
            DonorName = d.Donor != null
                ? (string.IsNullOrWhiteSpace(d.Donor.OrganizationName)
                    ? d.Donor.FirstName + " " + d.Donor.LastName
                    : d.Donor.OrganizationName)
                : string.Empty,
            DonationTypeId = d.DonationTypeId,
            DonationType = d.DonationType != null ? d.DonationType.Name : string.Empty,
            DonationStatusId = d.DonationStatusId,
            Status = d.DonationStatus != null ? d.DonationStatus.Name : string.Empty
        });
    }

    [HttpPost]
    public async Task<ActionResult<DonationDto>> CreateDonation(SaveDonationDto dto)
    {
        var donation = new Donation
        {
            DonationDate = dto.DonationDate,
            Amount = dto.Amount,
            ItemName = dto.ItemName,
            Quantity = dto.Quantity,
            EstimatedValue = dto.EstimatedValue,
            Notes = dto.Notes,
            DonorId = dto.DonorId,
            DonationTypeId = dto.DonationTypeId,
            DonationStatusId = dto.DonationStatusId
        };

        _context.Donations.Add(donation);
        await _context.SaveChangesAsync();

        return await GetDonationById(donation.Id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDonation(int id, SaveDonationDto dto)
    {
        var donation = await _context.Donations.FindAsync(id);

        if (donation is null)
            return NotFound();

        donation.DonationDate = dto.DonationDate;
        donation.Amount = dto.Amount;
        donation.ItemName = dto.ItemName;
        donation.Quantity = dto.Quantity;
        donation.EstimatedValue = dto.EstimatedValue;
        donation.Notes = dto.Notes;
        donation.DonorId = dto.DonorId;
        donation.DonationTypeId = dto.DonationTypeId;
        donation.DonationStatusId = dto.DonationStatusId;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDonation(int id)
    {
        var donation = await _context.Donations.FindAsync(id);

        if (donation is null)
            return NotFound();

        _context.Donations.Remove(donation);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
