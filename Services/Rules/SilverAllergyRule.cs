using CryptidCare.Models;

namespace CryptidCare.Services.Rules;

public class SilverAllergyRule : IClaimRule
{
    public void Apply(ClaimContext context)
    {
        if (context.Patient.Species == Species.Werewolf && context.Medicine.ContainsSilver)
            context.Reject("Medicine contains silver, which is contraindicated for Werewolf patients.");
    }
}
