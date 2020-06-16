using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Navyblue.Authorization.Authorizations.NavyblueResult
{
    public class NavyblueUnauthorizeResult : JsonResult
    {
        public NavyblueUnauthorizeResult(string message):base(new NavyblueError(message))
        {
            StatusCode = StatusCodes.Status403Forbidden;
        }
    }
}
