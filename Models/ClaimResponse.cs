namespace CryptidCare.Models;

public enum ClaimStatus
{
    Approved,
    Rejected
}

public record ClaimResponse(ClaimStatus Status, int DispensedQuantity, string? RejectionReason = null, Guid? ClaimId = null);
