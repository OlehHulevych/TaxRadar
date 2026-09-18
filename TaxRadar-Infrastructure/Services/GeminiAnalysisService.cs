using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Tax_Radar_Domain.Enums;
using Tax_Radar_Domain.Models;
using TaxRadar_Application.Exceptions;
using TaxRadar_Application.Interfaces;
using TaxRadar_Application.Models;
using TaxRadar_Infrastructure.Options;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace TaxRadar_Infrastructure.Services;

public class GeminiAnalysisService(HttpClient httpClient, IOptions<GeminiOptions> options) : IAiAnalysisService
{
    public async Task<ReceiptExtractionResult> ExtractReceiptDataAsync(byte[] imagesBytes, string mimeType,
        CancellationToken ct)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var requestBody = new
        {
            contents = new object[]
            {
                new
                {
                    parts = new object[]
                    {
                        new {text = $"You are analyzing a photo of a purchase receipt for a Czech OSVČ\n  (self-employed) bookkeeping app.\n  \n  Extract the following information from the receipt image and return it as\n  a single JSON object, with no markdown formatting, no code fences, and\n  no explanatory text before or after the JSON.\n\n  Fields:\n  - \"description\": string. A short description of the purchase (merchant\n  name and what was bought, e.g. \"Office supplies from Datart\"). Use null\n  if the receipt is too unclear to describe.\n  - \"amount\": number. The total amount paid, as a plain decimal number\n  (e.g. 1250.50). Use the final total, not a subtotal. Use null if no total\n  amount is legible.\n  - \"currency\": string. One of exactly: \"CZK\", \"EUR\", \"USD\". Use null if\n  the currency cannot be determined.\n  - \"expenseDate\": string. The date of the purchase in \"YYYY-MM-DD\" format.\n  Use null if no date is legible. Today's date is {today} — use this only\n  to resolve ambiguous partial dates, never as the receipt date itself.\n  - \"suggestedCategory\": string. Your best-guess category for this expense,\n  chosen from exactly this list: \"OfficeSupplies\", \"Software\",\n  \"Equipment\", \"Travel\", \"ProfessionalServices\", \"Marketing\", \"Utilities\",\n  \"Rent\", \"Education\", \"Other\". Use \"Other\" if none clearly fit. Never\n  invent a category outside this list.\n  - \"suggestedDeductibility\": string. Your best-guess assessment of whether\n  this expense is tax-deductible for a Czech self-employed person, chosen\n  from exactly: \"Deductible\", \"NonDeductible\", \"PartiallyDeductible\". If\n  you are not reasonably confident, use \"PartiallyDeductible\" rather than\n  guessing.\n\n  These are suggestions only, subject to human review — a wrong guess here\n  is expected and acceptable, but a fabricated amount or date is not. If a\n  field is not clearly legible in the image, use null for that field rather\n  than guessing a plausible-looking value.\n\n  Return only the JSON object."},
                        new
                        {
                            inlineData = new
                            {
                                mimeType = mimeType,
                                data = imagesBytes
                            }
                        }
                    },
                }
            }
        };

        var request = new HttpRequestMessage(HttpMethod.Post, options.Value.Url);
        request.Headers.Add("x-goog-api-key", $"{options.Value.Key}");
        var normalizedBody =  new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        request.Content = normalizedBody;
        var response = await httpClient.SendAsync(request,ct);
        if (!response.IsSuccessStatusCode) throw new BadRequestException("Failed to send Gemini request");
        var raw = await response.Content.ReadAsStringAsync(ct);
        var data = System.Text.Json.JsonDocument.Parse(raw);
        var text = data.RootElement.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0]
            .GetProperty("text").GetString();
        if (text == null) throw new InvalidOperationException("Failed get result"); 
        var result = JsonConvert.DeserializeObject<GeminiReceiptResponse>(text);
        var currency = Enum.TryParse<Currency>(result?.Currency, out var
            c) ? c : (Currency?)null;
       var category = Enum.TryParse<ExpenseCategory>(result?.SuggestedCategory, out var suggestedCategory)? suggestedCategory: (ExpenseCategory?) null;
       var deductibilityStatus =Enum.TryParse<DeductibilityStatus>(result?.SuggestedDeductibility, out var ds)?ds:(DeductibilityStatus?) null;
        var expenseDate = DateOnly.TryParse(result?.ExpenseDate, out var ed)?ed:(DateOnly?)null;
        return new ReceiptExtractionResult(
            result?.Description,
            result?.Amount,
            currency,
            expenseDate,
            category,
            deductibilityStatus

        );

    }
}