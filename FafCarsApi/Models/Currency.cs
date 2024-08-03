using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FafCarsApi.Data;
using FafCarsApi.Helpers;

namespace FafCarsApi.Models;

[Table("currencies")]
public class Currency {
  [Key]
  [Column("timestamp", TypeName = FafCarsDbContext.TimestampNoTimezoneSql)]
  public DateTime Timestamp { get; set; }

  [Column("data", TypeName = "json")]
  public Dictionary<string, CurrencyData> Data { get; set; } = [];
}
