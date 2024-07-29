using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FafCarsApi.Data;

namespace FafCarsApi.Models;

[Table("currencies")]
public class Currency {
  [Key]
  [Column("timestamp", TypeName = FafCarsDbContext.TimestampNoTimezoneSql)]
  public DateTime Timestamp { get; set; }
  
  [Column("eur")]
  public double Eur { get; set; }
  
  [Column("mdl")]
  public double Mdl { get; set; }
  
  [Column("ron")]
  public double Ron { get; set; }

  public const string BaseCurrency = "USD";

  public const string CurrenciesListString = "EUR,MDL,RON";
}
