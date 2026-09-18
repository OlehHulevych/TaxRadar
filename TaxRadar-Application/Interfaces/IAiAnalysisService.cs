using TaxRadar_Application.Models;

namespace TaxRadar_Application.Interfaces;

public interface IAiAnalysisService
{
    public Task<ReceiptExtractionResult> ExtractReceiptDataAsync(byte[] imagesBytes, string mimeType,
        CancellationToken ct);
}