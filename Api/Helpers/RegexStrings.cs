namespace Api.Helpers;

public struct RegexStrings {
  public const string Email = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
  public const string CountryCode = "^[a-zA-Z]{2}$";
  public const string Vin = "^[A-HJ-NPR-Z0-9]{17}$";
}
