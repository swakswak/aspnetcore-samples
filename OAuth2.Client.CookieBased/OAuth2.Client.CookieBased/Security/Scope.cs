namespace OAuth2.Client.Security;

public record Scope
{
    public string NameIdentifier { get; set; }
    public string Name { get; set; }
    public List<string> Others { get; set; }
}