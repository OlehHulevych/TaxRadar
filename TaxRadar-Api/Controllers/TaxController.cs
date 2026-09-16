using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaxRadar_Application.Queries.Tax;

namespace TaxRadar_backned.Controllers;

[ApiController]
[Route("api/tax")]
public class TaxController(ISender sender):ControllerBase
{
    [HttpGet("pausalni-dan-eligibility")]
    public async Task<IActionResult> GetPausalniDanEligibility([FromQuery] GetPausalniDanEligibilityQuery query)
    {
        var result = await sender.Send(query);
        return Ok(result);
    }

    [HttpGet("vat-registration-status")]
    public async Task<IActionResult> GetVatRegistrationStatus([FromQuery] GetVatRegistrationStatusQuery query)
    {
        var result = await sender.Send(query);
        return Ok(result);
    }
}
