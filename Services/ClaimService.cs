using CryptidCare.Models;
using CryptidCare.Services.Rules;

namespace CryptidCare.Services;

public class ClaimService : IClaimService
{
    private readonly ClaimRulesEngine _rulesEngine;

    public ClaimService(IEnumerable<IClaimRule> rules) => _rulesEngine = new ClaimRulesEngine(rules);

    public ClaimResponse ProcessClaim(Patient patient, Medicine medicine, int quantity)
    {
        var context = new ClaimContext
        {
            Patient = patient,
            Medicine = medicine,
            RequestedQuantity = quantity
        };

        _rulesEngine.Process(context);

        if (context.DispensedQuantity == 0)
            context.DispensedQuantity = context.RequestedQuantity;

        return new ClaimResponse(context.Status, context.DispensedQuantity, context.RejectionReason);
    }
}
