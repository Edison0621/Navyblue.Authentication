namespace Navyblue.Authorization.Authorizations.NavyblueResult;

public class NavyblueError
{
    public string Error { get; set; }

    public NavyblueError(string error)
    {
        this.Error = error;
    }
}