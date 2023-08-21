using Bogus;
using Dotnet.Rate.Limiter.Api.Context;
using Dotnet.Rate.Limiter.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Rate.Limiter.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CompanyController : ControllerBase
{
    private readonly AppDataContext _context;

    public CompanyController(AppDataContext context)
    {
        _context = context;
    }

    [HttpGet]
    [EnableRateLimiting("concurrency")]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var result = await _context.Company.ToListAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [EnableRateLimiting("sliding")]
    public async Task<IActionResult> CreateAsync()
    {
        try
        {
            var company = new Faker<CompanyEntity>()
                .RuleFor(u => u.Name, f => f.Company.CompanyName())
                .RuleFor(u => u.FantasyName, f => f.Company.CompanyName())
                .RuleFor(u => u.Owner, f => f.Name.FullName())
                .RuleFor(u => u.Document, f => f.Phone.PhoneNumber("##############"))
                .RuleFor(u => u.Address, f => $"{f.Address.StreetName()}, {f.Address.BuildingNumber()} - {f.Address.ZipCode()} - {f.Address.Country()}")
                .Generate();

            await _context.AddAsync(company);
            await _context.SaveChangesAsync();

            return Ok(company);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}