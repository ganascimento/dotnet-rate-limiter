using Dotnet.Rate.Limiter.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Rate.Limiter.Api.Context;

public class AppDataContext : DbContext
{
    public required DbSet<PersonEntity> Person { get; set; }
    public required DbSet<EmployeeEntity> Employee { get; set; }
    public required DbSet<CompanyEntity> Company { get; set; }

    public AppDataContext(DbContextOptions<AppDataContext> options) : base(options)
    { }
}