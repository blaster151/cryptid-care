using CryptidCare.Models;

namespace CryptidCare.Services;

public interface IClaimService
{
    ClaimResponse ProcessClaim(Patient patient, Medicine medicine, int quantity);
}
