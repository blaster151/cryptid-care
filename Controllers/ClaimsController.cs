using CryptidCare.Models;
using CryptidCare.Services;
using Microsoft.AspNetCore.Mvc;

namespace CryptidCare.Controllers;

[ApiController]
[Route("claims")]
public class ClaimsController : ControllerBase
{
    private readonly IClaimService _claimService;

    public ClaimsController(IClaimService claimService) => _claimService = claimService;

    [HttpPost]
    public ActionResult<ClaimResponse> SubmitClaim([FromBody] SubmitClaimRequest request)
    {
        // TODO: resolve Patient and Medicine from persistence by ID
        throw new NotImplementedException();
    }

    public ClaimResponse ProcessClaim(Patient patient, Medicine medicine, int quantity)
        => _claimService.ProcessClaim(patient, medicine, quantity);
}
