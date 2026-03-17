namespace Domain.Exceptions;

/// <summary>
/// Exception thrown when a domain invariant is violated.
/// </summary>
public class DomainException : Exception
{
    /// <summary>
    /// Initializes a new instance of <see cref="DomainException"/> with the specified message.
    /// </summary>
    public DomainException(string message) : base(message) { }
}
