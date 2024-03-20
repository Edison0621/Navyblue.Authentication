using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Navyblue.Authorization.Authorizations.NavyblueResult;

public class UnAuthorizeResult : JsonResult
{
    public UnAuthorizeResult(string message):base(new NavyblueError(message))
    {
        this.StatusCode = StatusCodes.Status401Unauthorized;
    }
}