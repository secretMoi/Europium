namespace Europium.Services.Apis;

public class BaseApiRepository
{
    protected HttpClient HttpClient = null!;

    protected CancellationToken GetCancellationToken(int delay)
    {
        return new CancellationTokenSource(new TimeSpan(0, 0, delay)).Token;
    } 
}