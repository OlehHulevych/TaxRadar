using MediatR;
using Tax_Radar_Domain.Enums;
using Tax_Radar_Domain.Models;
using TaxRadar_Application.Interfaces;
using TaxRadar_Application.Queries.Tax;
using TaxRadar_Application.Services;

namespace TaxRadar_Application.Commands.Tax;

public class GetVatRegistrationStatusQueryHandler(IInvoiceRepository invoiceRepository, ITaxRatesProvider taxRatesProvider):IRequestHandler<GetVatRegistrationStatusQuery, VatRegistrationStatus>
{
    public async Task<VatRegistrationStatus> Handle(GetVatRegistrationStatusQuery request, CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var rollingTwelveMonthTurnover = await invoiceRepository.GetTurnoverForPeriod(request.UserId,today.AddMonths(-12),today ,cancellationToken);
        var currentCalendarYearTurnover = await invoiceRepository.GetTurnoverForPeriod(request.UserId, new DateOnly(today.Year,1,1), today,cancellationToken);
        var rates = await taxRatesProvider.GetRatesForYearAsync(today.Year,cancellationToken);
        var status = VatThresholdEvaluator.Evaluate(rollingTwelveMonthTurnover, currentCalendarYearTurnover, rates);
        return status;
    }
}