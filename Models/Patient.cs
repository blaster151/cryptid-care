namespace CryptidCare.Models;

public enum Species
{
    Werewolf,
    Hydra,
    Phoenix,
    Other
}

public record Patient(Guid Id, string Name, Species Species, int? HeadCount = null);
