namespace Dotnet.Rate.Limiter.Api.Entities;

public class PersonEntity
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required DateTime BirthDate { get; set; }
    public required string MotherName { get; set; }
    public required string Phone { get; set; }
}