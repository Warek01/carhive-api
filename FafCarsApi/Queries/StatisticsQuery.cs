using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace FafCarsApi.Queries;

public class StatisticsQuery {
  [FromQuery(Name = "month")]
  [Range(0, 11)]
  public int Month { get; set; }
  
  [FromQuery(Name = "year")]
  [Range(2024, 2025)]
  public int Year { get; set; }
}
