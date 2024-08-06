using Api.Dtos.Request;
using Api.Helpers;
using Api.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Api.Controllers;

[ApiController]
[ApiVersion(1)]
[Authorize(Roles = AuthRoles.User)]
[Route("Api/v{v:apiVersion}/Report")]
public class ReportController(ReportService reportService) : Controller {
  [HttpPost]
  public async Task<ActionResult> GenerateReport(CreateReportDto dto) {
    Guid userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);
    
    return await reportService.CreateReport(dto, userId);
  }
}
