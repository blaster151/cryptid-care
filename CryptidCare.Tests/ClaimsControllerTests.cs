using CryptidCare.Controllers;
using CryptidCare.Data;
using CryptidCare.Data.Repositories;
using CryptidCare.Models;
using CryptidCare.Services;
using CryptidCare.Services.Rules;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CryptidCare.Tests;

public class ClaimsControllerTests : IDisposable
{
    private readonly ClaimsController _controller;
    private readonly SqliteConnection _connection;

    public ClaimsControllerTests()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        var services = new ServiceCollection();
        services.AddScoped<IClaimRule, SilverAllergyRule>();
        services.AddScoped<IClaimRule, HydraHeadMultiplierRule>();
        services.AddScoped<IClaimService, ClaimService>();
        services.AddDbContext<CryptidCareDbContext>(o =>
            o.UseSqlite(_connection));
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IMedicineRepository, MedicineRepository>();
        services.AddScoped<IClaimRepository, ClaimRepository>();
        services.AddScoped<ClaimsController>();

        var provider = services.BuildServiceProvider();
        provider.GetRequiredService<CryptidCareDbContext>().Database.EnsureCreated();
        _controller = provider.GetRequiredService<ClaimsController>();
    }

    public void Dispose() => _connection.Dispose();

    [Fact]
    public void ProcessClaim_RejectsWerewolf_WhenMedicineContainsSilver()
    {
        var werewolf = new Patient { Id = Guid.NewGuid(), Name = "Remus Lupin", Species = Species.Werewolf };
        var silverMed = new Medicine { Id = Guid.NewGuid(), Name = "Silver Sulfadiazine", ContainsSilver = true };

        var result = _controller.ProcessClaim(werewolf, silverMed, quantity: 1);

        Assert.Equal(ClaimStatus.Rejected, result.Status);
        Assert.NotNull(result.RejectionReason);
        Assert.Equal(0, result.DispensedQuantity);
    }

    [Fact]
    public void ProcessClaim_MultipliesQuantityByHeadCount_ForHydra()
    {
        var hydra = new Patient { Id = Guid.NewGuid(), Name = "Lernaean", Species = Species.Hydra, HeadCount = 3 };
        var medicine = new Medicine { Id = Guid.NewGuid(), Name = "Regeneron", ContainsSilver = false };

        var result = _controller.ProcessClaim(hydra, medicine, quantity: 2);

        Assert.Equal(ClaimStatus.Approved, result.Status);
        Assert.Equal(6, result.DispensedQuantity);
    }
}
