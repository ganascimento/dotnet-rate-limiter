namespace Dotnet.Rate.Limiter.Api.Entities;

public class EmployeeEntity
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required DateTime BirthDate { get; set; }
    public required string Document { get; set; }
    public required string Phone { get; set; }
    public required string Company { get; set; }
}