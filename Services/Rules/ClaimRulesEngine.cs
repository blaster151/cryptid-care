using CryptidCare.Models;

namespace CryptidCare.Services.Rules;

public class ClaimRulesEngine
{
    private readonly IEnumerable<IClaimRule> _rules;

    public ClaimRulesEngine(IEnumerable<IClaimRule> rules) => _rules = rules;

    public ClaimContext Process(ClaimContext context)
    {
        foreach (var rule in _rules)
        {
            rule.Apply(context);
            if (context.IsRejected) break;
        }
        return context;
    }
}
