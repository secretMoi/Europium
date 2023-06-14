using Renci.SshNet;

namespace Europium.Services.Ssh;

public class SshService
{
    private bool _disposed; // permet de savoir si la connexion a été disposed

    private static SshClient? _client; // contient la connexion
    private static bool _isConnecting;

    protected SshService(string host, string user, string password, int port = 22)
    {
        if(_client is null)
        {
            _client = new SshClient(host, port, user, password);
            _client.ConnectionInfo.Timeout = TimeSpan.FromSeconds(5);
        }

        _disposed = false;
    }

    protected async Task ConnectAsync()
    {
        if(IsConnected || _isConnecting) return;

        try
        {
            _isConnecting = true;
            await Task.Run(() =>
            {
                _client?.Connect();
                _isConnecting = false;
            });
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
        }
    }

    protected async Task<string?> RunCommandAsync(string command)
    {
        return await Task.Run(() =>
        {
            if (IsConnected && _client is not null) // si la connexion n'a pas été détruite entre temps
            {
                // si on a pas été déconnecté entre temps
                var sc = _client.CreateCommand(command);
                sc.Execute();
                return sc.Result;
            }

            Console.WriteLine("Ssh not connected");

            return null;
        });
    }

    private void Close()
    {
        if (!_disposed && _client is not null && _client.IsConnected)
            _client.Disconnect();
    }

    public void Dispose()
    {
        _client?.Dispose();

        _disposed = true;
    }

    private bool IsConnected => !_disposed && _client is not null && _client.IsConnected;
}
