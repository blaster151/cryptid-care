using CryptidCare.Models;
using Microsoft.AspNetCore.Mvc;

namespace CryptidCare.Controllers;

[ApiController]
[Route("claims")]
public class ClaimsController : ControllerBase
{
    [HttpPost]
    public ActionResult<ClaimResponse> SubmitClaim()
    {
        throw new NotImplementedException();
    }
}
