using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Navyblue.Authorization.Authorizations.NavyblueResult;

public class AuthorizationFilter : IAsyncAuthorizationFilter
{
    public readonly AuthorizationPolicy _policy;

    public AuthorizationFilter(AuthorizationPolicy policy)
    {
        this._policy = policy ?? throw new ArgumentNullException(nameof(policy) + "is null");
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context) + "is null");
        }

        if (context.ActionDescriptor.EndpointMetadata.Any(p=>p is IAllowAnonymous))
        {
            return;
        }

        if (!context.ActionDescriptor.EndpointMetadata.Any(p => p is AuthorizeAttribute))
        {
            return;
        }

        IPolicyEvaluator policyEvaluator = context.HttpContext.RequestServices.GetRequiredService<IPolicyEvaluator>();
        AuthenticateResult authenticateResult = await policyEvaluator.AuthenticateAsync(this._policy, context.HttpContext);
        PolicyAuthorizationResult authorizeResult = await policyEvaluator.AuthorizeAsync(this._policy, authenticateResult, context.HttpContext, context);

        if(authorizeResult.Challenged)
        {
            context.Result = new UnAuthorizeResult("Authorization Failed");
        }
        else if(authorizeResult.Forbidden)
        {
            context.Result = new ForbidResult(this._policy.AuthenticationSchemes.ToArray());
        }
    }
}