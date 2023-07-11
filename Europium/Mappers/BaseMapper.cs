namespace Europium.Mappers;

public class BaseMapper
{
    protected CancellationToken GetCancellationToken()
    {
        return new CancellationTokenSource(new TimeSpan(0, 0, 5)).Token;
    }
}