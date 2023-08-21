namespace Dotnet.Rate.Limiter.Api.Entities;

public class CompanyEntity
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string FantasyName { get; set; }
    public required string Document { get; set; }
    public required string Address { get; set; }
    public required string Owner { get; set; }
}