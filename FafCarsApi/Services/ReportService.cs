using AutoMapper;
using FafCarsApi.Data;
using FafCarsApi.Dtos.Request;
using FafCarsApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FafCarsApi.Services;

public class ReportService(
  FafCarsDbContext db,
  IMapper mapper
  ) {
  public async Task<ActionResult> CreateReport(CreateReportDto dto, Guid userId) {
    var report = mapper.Map<Report>(dto);
    report.UserId = userId;

    await db.Reports.AddAsync(report);
    await db.SaveChangesAsync();
    
    return new OkResult();
  }
}
