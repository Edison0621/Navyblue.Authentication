namespace Navyblue.Authorizations.Authorizations.NavyblueResult
{
    public class NavyblueError
    {
        public string Error { get; set; }

        public NavyblueError(string Error)
        {
            this.Error = Error;
        }
    }
}
