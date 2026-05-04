using CryptidCare.Models;

namespace CryptidCare.Services.Rules;

public class HydraHeadMultiplierRule : IClaimRule
{
    public void Apply(ClaimContext context)
    {
        if (context.Patient.Species == Species.Hydra && context.Patient.HeadCount is int heads)
            context.DispensedQuantity = context.RequestedQuantity * heads;
    }
}
