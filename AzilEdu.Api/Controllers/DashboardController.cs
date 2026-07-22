using AzilEdu.Api.Data;
using AzilEdu.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AzilEdu.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly AzilEduDbContext _context;

    public DashboardController(AzilEduDbContext context)
    {
        _context = context;
    }

    // Kasnije će admin vidjeti sve podatke, a ostale role samo svoj dio aplikacije.
    [HttpGet("summary")]
    public async Task<ActionResult<DashboardSummaryDto>> GetSummary()
    {
        var summary = new DashboardSummaryDto
        {
            AnimalsCount = await _context.Animals.CountAsync(),
            AvailableAnimalsCount = await _context.Animals.CountAsync(animal => animal.AnimalStatusId == 1),
            ActiveVolunteersCount = await _context.Volunteers.CountAsync(volunteer => volunteer.VolunteerStatusId == 2),
            OpenVolunteerTasksCount = await _context.VolunteerTasks.CountAsync(task => task.VolunteerTaskStatusId == 1),
            ActiveDonorsCount = await _context.Donors.CountAsync(donor => donor.DonorStatusId == 2),
            EmployeesCount = await _context.Employees.CountAsync(),

            DonationsCount = await _context.Donations.CountAsync(),
            PendingDonationsCount = await _context.Donations.CountAsync(d => d.DonationStatusId == 1),
            MoneyDonationsTotal = await _context.Donations
                .Where(d => d.DonationTypeId == 1 && d.Amount.HasValue)
                .SumAsync(d => d.Amount ?? 0),
            EstimatedMaterialDonationsTotal = await _context.Donations
                .Where(d => d.DonationTypeId != 1 && d.EstimatedValue.HasValue)
                .SumAsync(d => d.EstimatedValue ?? 0),
            OverdueVolunteerTasksCount = await _context.VolunteerTasks
                .CountAsync(t => t.DueDate.HasValue && t.DueDate.Value.Date < DateTime.Today
                                 && t.VolunteerTaskStatusId != 4 && t.VolunteerTaskStatusId != 5)
        };

        return Ok(summary);
    }

    [HttpGet("recent-donations")]
    public async Task<ActionResult<List<RecentDonationDto>>> GetRecentDonations()
    {
        var donations = await _context.Donations
            .Include(d => d.Donor)
            .Include(d => d.DonationType)
            .OrderByDescending(d => d.DonationDate)
            .Take(5)
            .Select(d => new RecentDonationDto
            {
                Id = d.Id,
                DonorName = d.Donor != null
                    ? (string.IsNullOrWhiteSpace(d.Donor.OrganizationName)
                        ? d.Donor.FirstName + " " + d.Donor.LastName
                        : d.Donor.OrganizationName)
                    : string.Empty,
                DonationType = d.DonationType != null ? d.DonationType.Name : string.Empty,
                DonationDate = d.DonationDate,
                Amount = d.Amount,
                ItemName = d.ItemName
            })
            .ToListAsync();

        return Ok(donations);
    }
}