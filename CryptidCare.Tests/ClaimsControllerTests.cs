using CryptidCare.Controllers;
using CryptidCare.Models;
using Microsoft.AspNetCore.Mvc;

namespace CryptidCare.Tests;

public class ClaimsControllerTests
{
    private readonly ClaimsController _controller = new();

    [Fact(Skip = "Not yet implemented")]
    public void SubmitClaim_ReturnsApproved_WhenClaimIsValid()
    {
        var result = _controller.SubmitClaim();

        var ok = Assert.IsType<ActionResult<ClaimResponse>>(result);
    }
}
