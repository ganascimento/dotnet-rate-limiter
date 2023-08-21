using Bogus;
using Dotnet.Rate.Limiter.Api.Context;
using Dotnet.Rate.Limiter.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Rate.Limiter.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly AppDataContext _context;

    public EmployeeController(AppDataContext context)
    {
        _context = context;
    }

    [HttpGet]
    [EnableRateLimiting("token")]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var result = await _context.Employee.ToListAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [EnableRateLimiting("token")]
    public async Task<IActionResult> CreateAsync()
    {
        try
        {
            var employee = new Faker<EmployeeEntity>()
                .RuleFor(u => u.Name, f => f.Name.FullName())
                .RuleFor(u => u.BirthDate, f => f.Date.Between(new DateTime(1950, 01, 01), new DateTime(2010, 12, 31)))
                .RuleFor(u => u.Phone, f => f.Phone.PhoneNumber("## #####-####"))
                .RuleFor(u => u.Document, f => f.Phone.PhoneNumber("###########"))
                .RuleFor(u => u.Company, f => f.Company.CompanyName())
                .Generate();

            await _context.AddAsync(employee);
            await _context.SaveChangesAsync();

            return Ok(employee);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}