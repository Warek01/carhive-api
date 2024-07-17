using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FafCarsApi.Helpers;

public class CustomSwaggerOperationFilter : IOperationFilter {
  public void Apply(OpenApiOperation operation, OperationFilterContext context) {
    // get roles at method level first.
    var roles = context.MethodInfo.GetCustomAttributes(true)
      .OfType<AuthorizeAttribute>()
      .Select(a => a.Roles)
      .Distinct()
      .ToArray();

    // we dont find roles at method level then check for controller level.
    if (roles.Length == 0) {
      roles = context.MethodInfo.DeclaringType?
        .GetCustomAttributes(true)
        .OfType<AuthorizeAttribute>()
        .Select(attr => attr.Roles)
        .Distinct()
        .ToArray();
    }

    string rolesStr = "None";

    if (roles != null && roles.Length != 0) {
      rolesStr = string.Join(", ", roles);
    }

    operation.Description = $"Required Roles: <b>{rolesStr}</b><br/><br/>" + operation.Description;
  }
}
