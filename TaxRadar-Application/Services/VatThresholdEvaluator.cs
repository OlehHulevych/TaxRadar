using Tax_Radar_Domain.Enums;
using Tax_Radar_Domain.Models;
using TaxYearRates = TaxRadar_Application.Models.TaxYearRates;

namespace TaxRadar_Application.Services;

public class VatThresholdEvaluator
{
    public static VatRegistrationStatus Evaluate(
        decimal rollingTwelveMonthTurnover,
        decimal currentCalendarYearTurnover,
        TaxYearRates rates)
    {
        if (currentCalendarYearTurnover > rates.VatImmediateThreshold)
            return new VatRegistrationStatus(true,
                VatRegistrationTrigger.ImmediateThreshold);

        if (rollingTwelveMonthTurnover > rates.VatThreshold)
            return new VatRegistrationStatus(true,
                VatRegistrationTrigger.RollingTwelveMonthThreshold);

        return new VatRegistrationStatus(false,
            VatRegistrationTrigger.None);
    }
}