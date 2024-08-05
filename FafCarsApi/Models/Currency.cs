using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FafCarsApi.Data;

namespace FafCarsApi.Models;

[Table("currencies")]
public class Currency {
  [Key]
  [Column("timestamp", TypeName = FafCarsDbContext.TimestampNoTimezoneSql)]
  public DateTime Timestamp { get; set; }

  [Column("data", TypeName = "json")]
  public Dictionary<string, double> Data { get; set; } = [];
}
