using MediatR;
using Tax_Radar_Domain.Models;
using TaxRadar_Application.Interfaces;
using TaxRadar_Application.Queries.Tax;
using TaxRadar_Application.Services;

namespace TaxRadar_Application.Commands.Tax;

public class GetPausalniDanEligibilityQueryHandler(ITaxRatesProvider taxRatesProvider, BandSelectionService bandSelectionService):IRequestHandler<GetPausalniDatEligibilityQuery, PausalniDanEligibility>
{
    public async Task<PausalniDanEligibility> Handle(GetPausalniDatEligibilityQuery request, CancellationToken cancellationToken)
    {
        var rates = await taxRatesProvider.GetRatesForYearAsync(request.Year,cancellationToken);
        return BandSelectionService.BandEvaluation(request.Income, rates);
    }
}