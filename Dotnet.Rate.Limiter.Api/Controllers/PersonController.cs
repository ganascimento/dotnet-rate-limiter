using Bogus;
using Dotnet.Rate.Limiter.Api.Context;
using Dotnet.Rate.Limiter.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Rate.Limiter.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PersonController : ControllerBase
{
    private readonly AppDataContext _context;

    public PersonController(AppDataContext context)
    {
        _context = context;
    }

    [HttpGet]
    [EnableRateLimiting("fixed")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await _context.Person.ToListAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [EnableRateLimiting("fixed")]
    public async Task<IActionResult> Create()
    {
        try
        {
            var person = new Faker<PersonEntity>()
                .RuleFor(u => u.Id, Guid.NewGuid())
                .RuleFor(u => u.Name, f => f.Name.FullName())
                .RuleFor(u => u.MotherName, f => f.Name.FullName(Bogus.DataSets.Name.Gender.Female))
                .RuleFor(u => u.BirthDate, f => f.Date.Between(new DateTime(1950, 01, 01), new DateTime(2010, 12, 31)))
                .RuleFor(u => u.Phone, f => f.Phone.PhoneNumber("## #####-####"))
                .Generate();

            await _context.Person.AddAsync(person);
            await _context.SaveChangesAsync();

            return Ok(person);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}