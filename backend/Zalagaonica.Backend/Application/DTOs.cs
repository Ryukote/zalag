// DTOs za Gemini i Instagram
namespace Zalagaonica.Application.DTOs;

public record ItemValuationDto(string SuggestedName, string EstimatedPriceRange, string? ErrorMessage = null);
public record ImageValuationRequestDto(List<string> ImageBase64List);
public record PublishItemDto(Guid ArticleId, string Caption, decimal Price);
public record PublishResponseDto(bool Success, string Message, string? PostUrl = null);

// Primjer DTO-a za klijenta (ostali slijede isti obrazac)
public record ClientDto(Guid Id, string Name, string City, string Address, string TaxId, string Email, string Status);
public record CreateClientDto(string Name, string City, string Address, string TaxId, string Email, string Iban, string Type, string Status);
public class CustomerDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? Status { get; set; }
}

public class ItemDataDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Value { get; set; }
    public string? Category { get; set; }
}

public class GeminiValuationDto
{
    public Guid Id { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public decimal EstimatedValue { get; set; }
    public string? Notes { get; set; }
    public string? Status { get; set; }
}

public class FileUploadDto
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string? ContentType { get; set; }
    public long Size { get; set; }
}

public class RevenueDto
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public string Source { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public string? ClientName { get; set; }
    public string? EmployeeName { get; set; }
}

public class PaymentDto
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public string Method { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public string? ClientName { get; set; }
    public string? EmployeeName { get; set; }
}

public class ExpenseDto
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public string? EmployeeName { get; set; }
}

public class RepaymentDto
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? Note { get; set; }
    public string? ClientName { get; set; }
    public string? EmployeeName { get; set; }
    public Guid? LoanId { get; set; }
}

//public class ProcjenaZahtjevDto
//{
//    public string NazivPredmeta { get; set; }
//    public string Stanje { get; set; }
//    public IFormFile[] Slike { get; set; }
//}


//public class ProcjenaOdgovorDto
//{
//    public decimal ProcjenaCijene { get; set; }
//    public string Poruka { get; set; }
//}


