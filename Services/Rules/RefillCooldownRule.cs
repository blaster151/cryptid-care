using CryptidCare.Models;

namespace CryptidCare.Services.Rules;

public class RefillCooldownRule : IClaimRule
{
    public const int CooldownDays = 30;

    public void Apply(ClaimContext context)
    {
        if (context.RecentApprovedClaims.Any())
            context.Reject($"This medicine was already dispensed within the last {CooldownDays} days. Refills are not permitted until the cooldown period has elapsed.");
    }
}
