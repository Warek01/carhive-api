using System.ComponentModel.DataAnnotations;
using FafCarsApi.Enums;

namespace FafCarsApi.Dtos.Request;

public class CreateReportDto {
  public Guid ListingId { get; set; }
  
  [StringLength(500)]
  public string? Reason { get; set; }
  
  public ReportType Type { get; set; }
}
