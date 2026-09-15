using TaxRadar_Application.Models;
using TaxRadar_Application.Services;
using Tax_Radar_Domain.Enums;
using Xunit;

namespace TaxRadar_Application.Tests.Services;

public class VatThresholdEvaluatorTests
{
    private static readonly TaxYearRates Rates2026 = new(
        2026,
        VatThreshold: 2_000_000m,
        VatImmediateThreshold: 2_536_500m,
        PausalniDanBand1: 9_162m,
        PausalniDanBand2: 16_745m,
        PausalniDanBand3: 27_139m,
        PausalniDanBand1IncomeLimit: 1_000_000m,
        PausalniDanBand2IncomeLimit: 1_500_000m,
        PausalniDanBand3IncomeLimit: 2_000_000m);

    [Fact]
    public void RollingTurnover_ExactlyAtThreshold_DoesNotRequireRegistration()
    {
        var result = VatThresholdEvaluator.Evaluate(
            rollingTwelveMonthTurnover: 2_000_000m,
            currentCalendarYearTurnover: 0m,
            rates: Rates2026);

        Assert.False(result.MustRegister);
        Assert.Equal(VatRegistrationTrigger.None, result.Trigger);
    }

    [Fact]
    public void RollingTurnover_OneCentOverThreshold_RequiresRegistration()
    {
        var result = VatThresholdEvaluator.Evaluate(
            rollingTwelveMonthTurnover: 2_000_000.01m,
            currentCalendarYearTurnover: 0m,
            rates: Rates2026);

        Assert.True(result.MustRegister);
        Assert.Equal(VatRegistrationTrigger.RollingTwelveMonthThreshold, result.Trigger);
    }

    [Fact]
    public void CalendarYearTurnover_ExactlyAtImmediateThreshold_DoesNotRequireRegistration()
    {
        var result = VatThresholdEvaluator.Evaluate(
            rollingTwelveMonthTurnover: 0m,
            currentCalendarYearTurnover: 2_536_500m,
            rates: Rates2026);

        Assert.False(result.MustRegister);
        Assert.Equal(VatRegistrationTrigger.None, result.Trigger);
    }

    [Fact]
    public void CalendarYearTurnover_OneCentOverImmediateThreshold_RequiresImmediateRegistration()
    {
        var result = VatThresholdEvaluator.Evaluate(
            rollingTwelveMonthTurnover: 0m,
            currentCalendarYearTurnover: 2_536_500.01m,
            rates: Rates2026);

        Assert.True(result.MustRegister);
        Assert.Equal(VatRegistrationTrigger.ImmediateThreshold, result.Trigger);
    }

    [Fact]
    public void BothThresholdsExceeded_ImmediateThresholdTakesPriority()
    {
        var result = VatThresholdEvaluator.Evaluate(
            rollingTwelveMonthTurnover: 3_000_000m,
            currentCalendarYearTurnover: 2_600_000m,
            rates: Rates2026);

        Assert.True(result.MustRegister);
        Assert.Equal(VatRegistrationTrigger.ImmediateThreshold, result.Trigger);
    }

    [Fact]
    public void TurnoverWellBelowBothThresholds_DoesNotRequireRegistration()
    {
        var result = VatThresholdEvaluator.Evaluate(
            rollingTwelveMonthTurnover: 500_000m,
            currentCalendarYearTurnover: 500_000m,
            rates: Rates2026);

        Assert.False(result.MustRegister);
        Assert.Equal(VatRegistrationTrigger.None, result.Trigger);
    }
}
