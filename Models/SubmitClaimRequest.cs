namespace CryptidCare.Models;

public record SubmitClaimRequest(
    Guid PatientId,
    Guid MedicineId,
    int Quantity,
    string? ExternalReferenceId = null
);
