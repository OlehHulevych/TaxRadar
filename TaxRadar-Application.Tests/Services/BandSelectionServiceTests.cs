using TaxRadar_Application.Models;
using TaxRadar_Application.Services;
using Tax_Radar_Domain.Enums;
using Xunit;

namespace TaxRadar_Application.Tests.Services;

public class BandSelectionServiceTests
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
    public void LowIncome_IsEligibleForCheapestBand()
    {
        var result = BandSelectionService.BandEvaluation(500_000m, Rates2026);

        Assert.True(result.IsEligible);
        Assert.Equal(PausalniDanBand.Band1, result.SuggestedBand);
    }

    [Fact]
    public void Income_ExactlyAtBand1Limit_IsBand1()
    {
        var result = BandSelectionService.BandEvaluation(1_000_000m, Rates2026);

        Assert.True(result.IsEligible);
        Assert.Equal(PausalniDanBand.Band1, result.SuggestedBand);
    }

    [Fact]
    public void Income_OneCentOverBand1Limit_IsBand2()
    {
        var result = BandSelectionService.BandEvaluation(1_000_000.01m, Rates2026);

        Assert.True(result.IsEligible);
        Assert.Equal(PausalniDanBand.Band2, result.SuggestedBand);
    }

    [Fact]
    public void Income_ExactlyAtBand2Limit_IsBand2()
    {
        var result = BandSelectionService.BandEvaluation(1_500_000m, Rates2026);

        Assert.True(result.IsEligible);
        Assert.Equal(PausalniDanBand.Band2, result.SuggestedBand);
    }

    [Fact]
    public void Income_OneCentOverBand2Limit_IsBand3()
    {
        var result = BandSelectionService.BandEvaluation(1_500_000.01m, Rates2026);

        Assert.True(result.IsEligible);
        Assert.Equal(PausalniDanBand.Band3, result.SuggestedBand);
    }

    [Fact]
    public void Income_ExactlyAtBand3Limit_IsBand3()
    {
        var result = BandSelectionService.BandEvaluation(2_000_000m, Rates2026);

        Assert.True(result.IsEligible);
        Assert.Equal(PausalniDanBand.Band3, result.SuggestedBand);
    }

    [Fact]
    public void Income_OneCentOverBand3Limit_IsNotEligible()
    {
        var result = BandSelectionService.BandEvaluation(2_000_000.01m, Rates2026);

        Assert.False(result.IsEligible);
        Assert.Equal(PausalniDanBand.NotEligible, result.SuggestedBand);
    }
}
