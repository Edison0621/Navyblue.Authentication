namespace Navyblue.Authorization.Authorizations.NavyblueResult;

public class NavyblueError(string error)
{
    public string Error { get; set; } = error;
}