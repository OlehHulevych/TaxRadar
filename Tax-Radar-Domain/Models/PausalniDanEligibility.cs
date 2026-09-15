using Tax_Radar_Domain.Enums;

namespace Tax_Radar_Domain.Models;

public record PausalniDanEligibility(bool IsEligible, PausalniDanBand SuggestedBand);