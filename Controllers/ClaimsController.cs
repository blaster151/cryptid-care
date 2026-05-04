using CryptidCare.Data.Repositories;
using CryptidCare.Models;
using CryptidCare.Services;
using Microsoft.AspNetCore.Mvc;

namespace CryptidCare.Controllers;

[ApiController]
[Route("claims")]
public class ClaimsController : ControllerBase
{
    private readonly IClaimService _claimService;
    private readonly IPatientRepository _patients;
    private readonly IMedicineRepository _medicines;
    private readonly IClaimRepository _claims;

    public ClaimsController(
        IClaimService claimService,
        IPatientRepository patients,
        IMedicineRepository medicines,
        IClaimRepository claims)
    {
        _claimService = claimService;
        _patients = patients;
        _medicines = medicines;
        _claims = claims;
    }

    [HttpPost]
    public async Task<ActionResult<ClaimResponse>> SubmitClaim([FromBody] SubmitClaimRequest request)
    {
        var patient = await _patients.GetByIdAsync(request.PatientId);
        if (patient is null)
            return NotFound($"Patient '{request.PatientId}' not found.");

        var medicine = await _medicines.GetByIdAsync(request.MedicineId);
        if (medicine is null)
            return NotFound($"Medicine '{request.MedicineId}' not found.");

        if (!Enum.IsDefined(patient.Species))
            return UnprocessableEntity($"Patient '{patient.Id}' has unrecognized species value '{(int)patient.Species}'.");

        var result = ProcessClaim(patient, medicine, request.Quantity);

        var claimId = Guid.NewGuid();
        await _claims.AddAsync(new Claim
        {
            Id = claimId,
            PatientId = patient.Id,
            MedicineId = medicine.Id,
            ExternalReferenceId = request.ExternalReferenceId,
            RequestedQuantity = request.Quantity,
            DispensedQuantity = result.DispensedQuantity,
            Status = result.Status,
            RejectionReason = result.RejectionReason,
            CreatedAt = DateTime.UtcNow
        });

        return Ok(result with { ClaimId = claimId });
    }

    [NonAction]
    public ClaimResponse ProcessClaim(Patient patient, Medicine medicine, int quantity)
        => _claimService.ProcessClaim(patient, medicine, quantity);
}
