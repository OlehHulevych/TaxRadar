using Tax_Radar_Domain.Enums;

namespace Tax_Radar_Domain.Models;

public sealed record VatRegistrationStatus(bool MustRegister, VatRegistrationTrigger Trigger);