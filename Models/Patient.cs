namespace CryptidCare.Models;

public enum Species
{
    Werewolf,
    Hydra,
    Phoenix,
    Other
}

public class Patient
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Species Species { get; set; }
    public int? HeadCount { get; set; }
}
