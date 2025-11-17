using Types;

using FinancicalSystem;
using SecuritySystems;
using VisitorManagment;
using PersonalManagment;
namespace Exceptions
{

public class ArtworkNotFoundException : Exception
{
    public ArtworkNotFoundException(int artworkId)
        : base($"Artwork with ID {artworkId} not found") { }
}

public class InvalidArtworkConditionException : Exception
{
    public InvalidArtworkConditionException(string condition)
        : base($"Invalid artwork condition: {condition}") { }
}

public class InsufficientFundsException : Exception
{
    public InsufficientFundsException(decimal amount, decimal balance)
        : base($"Insufficient funds. Required: {amount}, Available: {balance}") { }
}

public class InvalidTransactionAmountException : Exception
{
    public InvalidTransactionAmountException()
        : base("Transaction amount must be non-zero") { }
}

public class SecurityBreachException : Exception
{
    public SecurityBreachException(string message) : base(message) { }
}

public class UnauthorizedAccessException : Exception
{
    public UnauthorizedAccessException(string role)
        : base($"Access denied for role: {role}") { }
}

public class ExhibitionFullException : Exception
{
    public ExhibitionFullException(string exhibitionName)
        : base($"Exhibition '{exhibitionName}' is at full capacity") { }
}

public class ArtworkNotAvailableException : Exception
{
    public ArtworkNotAvailableException(int artworkId)
        : base($"Artwork {artworkId} is not available for exhibition") { }
}

public class InvalidTicketException : Exception
{
    public InvalidTicketException(string ticketCode)
        : base($"Invalid or expired ticket: {ticketCode}") { }
}

public class EmployeeNotFoundException : Exception
{
    public EmployeeNotFoundException(int employeeId)
        : base($"Employee with ID {employeeId} not found") { }
}

public class HallCapacityExceededException : Exception
{
    public HallCapacityExceededException(string hallName)
        : base($"Hall '{hallName}' capacity exceeded") { }
}

public class RestorationNotPossibleException : Exception
{
    public RestorationNotPossibleException(string reason)
        : base($"Restoration not possible: {reason}") { }
}
}