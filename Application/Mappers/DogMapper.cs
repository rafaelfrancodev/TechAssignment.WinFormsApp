using Application.DTOs;
using Domain.Entities;

namespace Application.Mappers;

/// <summary>
/// Maps <see cref="Dog"/> domain entities to DTOs.
/// </summary>
public static class DogMapper
{
    /// <summary>
    /// Converts a Dog entity to a DogDto, including client name.
    /// </summary>
    public static DogDto ToDto(Dog dog, string clientName) =>
        new(dog.Id, dog.ClientId, clientName, dog.Name, dog.Breed, dog.Age);
}
