namespace Europium.Repositories;

public class BaseApiRepository
{
    protected HttpClient HttpClient;

    protected BaseApiRepository() => HttpClient = new HttpClient(new HttpClientHandler());

    protected CancellationToken GetCancellationToken(int delay) => new CancellationTokenSource(new TimeSpan(0, 0, delay)).Token;
}