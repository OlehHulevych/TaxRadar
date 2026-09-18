using Tax_Radar_Domain.Enums;

namespace TaxRadar_Application.Models;

public record ReceiptExtractionResult(string? Description, decimal? Amount, Currency? Currency, DateOnly? ExpenseDate, ExpenseCategory? SuggestedCategory, DeductibilityStatus? SuggestedDeductibility);
