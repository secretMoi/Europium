namespace Europium.Repositories.FlareSolver.Models;

public class FlareSolverCommandFactory
{
    public FlareSolverCommand GetRequestCommand(string url, string sessionName)
    {
        return new FlareSolverCommand
        {
            Command = "request.get",
            Url = url,
            Session = sessionName,
            ReturnOnlyCookies = true,
            MaxTimeout = 15000
        };
    }
    
    public FlareSolverCommand GetSessionsListCommand()
    {
        return new FlareSolverCommand
        {
            Command = "sessions.list"
        };
    }
    
    public FlareSolverCommand GetCreateSessionCommand(string sessionName)
    {
        return new FlareSolverCommand
        {
            Command = "sessions.create",
            Session = sessionName
        };
    }
    
    public FlareSolverCommand GetRemoveSessionCommand(string sessionName)
    {
        return new FlareSolverCommand
        {
            Command = "sessions.destroy",
            Session = sessionName
        };
    }
}