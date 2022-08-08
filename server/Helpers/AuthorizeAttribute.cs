using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TodoApi.Models;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
  public void OnAuthorization(AuthorizationFilterContext context)
  {
    var user = context.HttpContext.Items["User"];
    if (user == null)
    {
      context.Result = new JsonResult(new { message = "Unauthorized", status = 401 });
    }
  }
}