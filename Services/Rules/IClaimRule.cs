using CryptidCare.Models;

namespace CryptidCare.Services.Rules;

public interface IClaimRule
{
    void Apply(ClaimContext context);
}
