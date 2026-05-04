namespace CryptidCare.Models;

public class Medicine
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool ContainsSilver { get; set; }
}
