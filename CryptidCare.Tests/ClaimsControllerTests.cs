using CryptidCare.Controllers;
using CryptidCare.Models;
using CryptidCare.Services;
using CryptidCare.Services.Rules;
using Microsoft.Extensions.DependencyInjection;

namespace CryptidCare.Tests;

public class ClaimsControllerTests
{
    private readonly ClaimsController _controller;

    public ClaimsControllerTests()
    {
        var services = new ServiceCollection();
        services.AddScoped<IClaimRule, SilverAllergyRule>();
        services.AddScoped<IClaimRule, HydraHeadMultiplierRule>();
        services.AddScoped<IClaimService, ClaimService>();
        services.AddScoped<ClaimsController>();
        _controller = services.BuildServiceProvider().GetRequiredService<ClaimsController>();
    }

    [Fact]
    public void ProcessClaim_RejectsWerewolf_WhenMedicineContainsSilver()
    {
        var werewolf = new Patient(Guid.NewGuid(), "Remus Lupin", Species.Werewolf);
        var silverMed = new Medicine(Guid.NewGuid(), "Silver Sulfadiazine", ContainsSilver: true);

        var result = _controller.ProcessClaim(werewolf, silverMed, quantity: 1);

        Assert.Equal(ClaimStatus.Rejected, result.Status);
        Assert.NotNull(result.RejectionReason);
    }

    [Fact]
    public void ProcessClaim_MultipliesQuantityByHeadCount_ForHydra()
    {
        var hydra = new Patient(Guid.NewGuid(), "Lernaean", Species.Hydra, HeadCount: 3);
        var medicine = new Medicine(Guid.NewGuid(), "Regeneron", ContainsSilver: false);

        var result = _controller.ProcessClaim(hydra, medicine, quantity: 2);

        Assert.Equal(ClaimStatus.Approved, result.Status);
        Assert.Equal(6, result.DispensedQuantity);
    }
}
