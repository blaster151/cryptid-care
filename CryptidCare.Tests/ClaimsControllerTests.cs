using CryptidCare.Controllers;
using CryptidCare.Models;

namespace CryptidCare.Tests;

public class ClaimsControllerTests
{
    private readonly ClaimsController _controller = new();

    [Fact]
    public void ProcessClaim_RejectsWerewolf_WhenMedicineContainsSilver()
    {
        var werewolf = new Patient(Guid.NewGuid(), "Remus Lupin", Species.Werewolf);
        var silverMed = new Medicine(Guid.NewGuid(), "Silver Sulfadiazine", ContainsSilver: true);

        var result = _controller.ProcessClaim(werewolf, silverMed, quantity: 1);

        Assert.Equal(ClaimStatus.Rejected, result.Status);
        Assert.NotNull(result.RejectionReason);
    }
}
