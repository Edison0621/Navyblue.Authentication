using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Navyblue.Authorization.Authorizations.NavyblueResult
{
    public class NavyblueUnAuthorizeResult : JsonResult
    {
        public NavyblueUnAuthorizeResult(string message):base(new NavyblueError(message))
        {
            this.StatusCode = StatusCodes.Status401Unauthorized;
        }
    }
}
