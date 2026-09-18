namespace Tax_Radar_Domain.Models;

public record GeminiReceiptResponse(
    string? Description,
    decimal? Amount,
    string? Currency,
    string? ExpenseDate,
    string? SuggestedCategory,
    string? SuggestedDeductibility
    );