namespace CryptidCare.Models;

public class Claim
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public Guid MedicineId { get; set; }
    public string? ExternalReferenceId { get; set; }
    public int RequestedQuantity { get; set; }
    public int DispensedQuantity { get; set; }
    public ClaimStatus Status { get; set; }
    public string? RejectionReason { get; set; }
    public DateTime CreatedAt { get; set; }

    public Patient Patient { get; set; } = null!;
    public Medicine Medicine { get; set; } = null!;
}
