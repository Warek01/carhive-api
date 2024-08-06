using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Api.Data;

namespace Api.Models;

[Table("currencies")]
public class Currency {
  [Key]
  [Column("timestamp", TypeName = CarHiveDbContext.TimestampNoTimezoneSql)]
  public DateTime Timestamp { get; set; }

  [Column("data", TypeName = "json")]
  public Dictionary<string, double> Data { get; set; } = [];
}
