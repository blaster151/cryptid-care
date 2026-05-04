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
        var medicine = await _medicines.GetByIdAsync(request.MedicineId);

        var result = ProcessClaim(patient!, medicine!, request.Quantity);

        await _claims.AddAsync(new Claim
        {
            Id = Guid.NewGuid(),
            PatientId = patient!.Id,
            MedicineId = medicine!.Id,
            ExternalReferenceId = request.ExternalReferenceId,
            RequestedQuantity = request.Quantity,
            DispensedQuantity = result.DispensedQuantity,
            Status = result.Status,
            RejectionReason = result.RejectionReason,
            CreatedAt = DateTime.UtcNow
        });

        return Ok(result);
    }

    public ClaimResponse ProcessClaim(Patient patient, Medicine medicine, int quantity)
        => _claimService.ProcessClaim(patient, medicine, quantity);
}
