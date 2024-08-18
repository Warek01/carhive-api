namespace Api.Helpers;

public class JwtConfig {
  public int Ttl { get; set; }
  public string Audience { get; set; } = null!;
  public string Issuer { get; set; } = null!;
  public int RefreshTtl { get; set; }
  public string Key { get; set; } = null!;
}
