using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navyblue.Authorizations.Authorizations.NavyblueResult;

namespace Navyblue.Authorizations.Authorizations.CustomResult
{
    public class NavyblueUnauthorizeResult : JsonResult
    {
        public NavyblueUnauthorizeResult(string message):base(new NavyblueError(message))
        {
            StatusCode = StatusCodes.Status403Forbidden;
        }
    }
}
