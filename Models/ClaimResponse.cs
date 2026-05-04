namespace CryptidCare.Models;

public enum ClaimStatus
{
    Approved,
    Rejected
}

public record ClaimResponse(ClaimStatus Status, string? RejectionReason = null);
