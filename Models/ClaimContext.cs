namespace CryptidCare.Models;

public class ClaimContext
{
    public required Patient Patient { get; init; }
    public required Medicine Medicine { get; init; }
    public required int RequestedQuantity { get; init; }

    // Explicitly set by rules (e.g. Hydra multiplier); 0 until assigned
    public int DispensedQuantity { get; set; }

    public ClaimStatus Status { get; private set; } = ClaimStatus.Approved;
    public string? RejectionReason { get; private set; }
    public bool IsRejected => Status == ClaimStatus.Rejected;

    public void Reject(string reason)
    {
        Status = ClaimStatus.Rejected;
        RejectionReason = reason;
    }
}
