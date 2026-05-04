using CryptidCare.Models;
using Microsoft.AspNetCore.Mvc;

namespace CryptidCare.Controllers;

[ApiController]
[Route("claims")]
public class ClaimsController : ControllerBase
{
    [HttpPost]
    public ActionResult<ClaimResponse> SubmitClaim([FromBody] SubmitClaimRequest request)
    {
        // TODO: resolve Patient and Medicine from persistence by ID
        throw new NotImplementedException();
    }

    public ClaimResponse ProcessClaim(Patient patient, Medicine medicine, int quantity)
    {
        throw new NotImplementedException();
    }
}
