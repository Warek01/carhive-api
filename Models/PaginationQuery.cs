using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace FafCarsApi.Models;

public class PaginationQuery
{
  [FromQuery(Name = "page")]
  [Range(0, int.MaxValue)]
  [DefaultValue(0)]
  public int Page { get; set; }

  [FromQuery(Name = "take")]
  [Range(0, int.MaxValue)]
  [DefaultValue(10)]
  public int Take { get; set; }
}
