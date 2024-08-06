using Api.Data;
using Api.Dtos.Request;
using Api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Api.Services;

public class ReportService(
  CarHiveDbContext db,
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
