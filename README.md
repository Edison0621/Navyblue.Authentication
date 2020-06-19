# Navyblue.Authentication
The package is for developer to create auth function module, you just only need to configure little information and then your api will be protected by this package.

# Detals
- Setting user identifier is '111111111111111111111111111111111'
![sample](/Images/sample.png)

- Getting token
![gettoken](/Images/gettoken.png)

- Validate
![validate](/Images/validate.png)

- Validate2, we will get the user identifier
![validate2](/Images/validate2.png)

- Forbid
![validate](/Images/error.png)

# How to use it
- In Startup.ConfigureServices
```
services.AddBearerService(this.Configuration.GetSection("NavyAuthorizationConfig"));
```
- In Startup.Configure
```
app.AddBearer();
```
- Add AuthorizationController and inherits AuthApiController
```
/// <summary>
/// Class AuthController.
/// Implements the <see cref="Microsoft.AspNetCore.Mvc.Controller" />
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
[Route("[controller]")]
public class AuthController : AuthApiController
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    /// <param name="authConfigOptions">The authentication configuration options.</param>
    public AuthController(IOptions<AuthorizationConfig> authConfigOptions):base(authConfigOptions)
    {
    }

    /// <summary>
    /// Gets the authentication token.
    /// </summary>
    /// <returns>IActionResult.</returns>
    [HttpGet]
    [AllowAnonymous]
    [Route("GetAuthToken")]
    public IActionResult GetAuthToken()
    {
        string userIdentify = "111111111111111111111111111111111";
        string authToken = this.BuildAuthToken(userIdentify, AuthorizationScheme.Bearer);
        return this.Ok(authToken);
    }

    /// <summary>
    /// Gets the authentication token.
    /// </summary>
    /// <returns>IActionResult.</returns>
    [HttpGet]
    [UserAuthorize]
    [Route("TestAuthToken")]
    public IActionResult TestAuthToken()
    {
        return this.Ok("Tested");
    }
}
```
- The configuration structure like this:
```
  "NavyAuthorizationConfig": {
    "AuthHeaderName": "x-authentication",
    "AndroidClientId": "901",
    "IOSClientId": "902",
    "UseSwaggerAsApplicationForDev": "false",
    "IPAllowedLists": [],
    "GovernmentServerPublicKey": "",
    "BearerAuthKeys": "&lt;RSAKeyValue&gt;&lt;Modulus&gt;3o+HBruoycocmkwreug2DJLWzGl7EB0X7xTVvUOauVeX8O5t47jbllacZuv691W85pGO8ng6hQDiBvXtUz2uGJ5k8V6x2xbiK1qtMMP7QMxfLcg5zCB6i4RXFfqA5PXEtJt8S9mOk92rws1BoF3cSA9f7rNyqWToMYD+oGlATuxv5+PRr24HBm9w5eSxb24HvJbThMnUg0leLr77VY1LEOkUNn5TIQ38Y0Wo9gkwrCMLPXLLZqEU46sqAykhrvDOWtTczrBeRkqUICflmSK0OD0H6O958PwPzHW2h6mpFle4NQpP+QDmNbmK4zv7j71veelcuKV0WUcbXe/hfYSXzw==&lt;/Modulus&gt;&lt;Exponent&gt;AQAB&lt;/Exponent&gt;&lt;P&gt;7Qo+FrkpSBOuNUba7KoEW+L0yU8W+AETxeOVLST4Ugas/NlBnbPy/JsziIHp0IXz3f5HXVy+P/DPbztkCNXD3awyMNRQ2z+Otr90ybTQ5DMShugKRPGFtPs4m6ALtN8YSgzQJuK0BFB4d07IX0k3KHEqEob2UGcKT8Ogn4loDXE=&lt;/P&gt;&lt;Q&gt;8FzMQ/2F/O9UxRs5QMxCqFpuLE466iDY3oyZmwdOqKOn0uXs1UiVGXRcubBcGiQVbmMcCLlvogohjYp5mUU9Z1gTI6iklvOzoEOdxb8n92FwlOcZGval7L4DmHjLH2HFDjM4djNjAKdJngDg4r938QZIqONXSGeg3g06EymNWT8=&lt;/Q&gt;&lt;DP&gt;ey8ZeSGcjHJ90/4Qg3EPdtkJMRzC6PtWVT6iJaXSzn3dpEEbUmNT4Waeb1BkPBOA2lrsp14tGHmCs2F/6P9+HFMCelG7+1SaS+pPPQuUiyLne+hWfeGuBJGRp36S1tohe0oRWkPyHVPcZtQwWSRpX8D/hkVQ+BO0TiNx87aqtmE=&lt;/DP&gt;&lt;DQ&gt;Idc+3xezpJ/hlHq3vdrES8Wnm09MVihXwEWVXtFRjsaz2yqKBKFadKIAaBhfb0LDTa5ghQ3unKbGgJINernX4lPxJeUZfzNCh/7dGLlIHDk4y44Z58TwKXu7L91Z48o1H0Gw4ltrxezHnZpMD0Cb13BmDHktkcEdUgYUthv0jas=&lt;/DQ&gt;&lt;InverseQ&gt;MT1wLbYdxmf995ZTCSJQgyGgFSqr3Fkdt/wYJP9VMUnYKp6PPHzTOs3urul6YIs9GWQ+hl7JBiqOQgUPsSc2WkUt2q/3kf7eKnzJiFrz8sOgL4sU7oxKgUW+i0wnfgvRmbU2R8b6vFBkfI6HFtdhRqzo5llZFe8/+Gl1ZK7SYGY=&lt;/InverseQ&gt;&lt;D&gt;ZD113fD4sUYwQsiqzrU23svmJfQeQuAvrvWN3SxNTEwo+ZGR+f6BHIHO+MYxZ2P87EZEAW5oQQ5oPyVV6md9+cWhhlsVtS1l2YwCNFQY3pMODVNAwPEh8KO/C32jvzv4iDX9sjX/MxckrN0AwWd09xnBpgO50ZTKxb0pMfulxMV/Dbykip10SdF4wZ7RSg8p0ol8WSwbhYGbgTp2aJEsVj9SZQCbXifW2P37lpFlfz20Sm4+vhDcrZhpFtHh2LILA+sqxVDw0hOkXuyEu8FQu1FahzX3xd+GrG77EcowpSP0gyfsN6qKUfwaP0jW+f7Ze0uOFHiav5/CSwhu5jm/MQ==&lt;/D&gt;&lt;/RSAKeyValue&gt;",
    "PCSignInExpirationSeconds": 86400,
    "AppSignInExpirationSeconds": 86400,
    "MobileSignInExpirationSeconds": 86400
  }
```
# Release Note
- 1.0.2

Add [ApiController] to AuthApiController as default setting

- 1.0.1

Abstract token generating to AuthApiController

- 1.0.0

Initial project
