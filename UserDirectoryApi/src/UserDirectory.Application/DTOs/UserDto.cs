#nullable enable
namespace UserDirectory.Application.DTOs;

public record UserDto(int Id, string Name, int Age, string City, string State, string Pincode);
