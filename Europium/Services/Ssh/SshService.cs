﻿using Renci.SshNet;

namespace Europium.Services.Ssh;

public class SshService
{
    private bool _disposed; // permet de savoir si la connexion a été disposed

    private static SshClient client; // contient la connexion

    protected SshService(string host, string user, string password, int port = 22)
    {
        if(client is null)
            client = new SshClient(host, port, user, password);

        _disposed = false;
    }

    protected async Task ConnectAsync()
    {
        if(!_disposed && client.IsConnected)
            return;

        try
        {
            await Task.Run(() => client.Connect());
        }
        catch
        {
        }
    }

    protected async Task<string> RunCommandAsync(string command)
    {
        return await Task.Run(() =>
        {
            if (client is not null && !_disposed && client.IsConnected) // si la connexion n'a pas été détruite entre temps
            {
                // si on a pas été déconnecté entre temps
                var sc = client.CreateCommand(command);
                sc.Execute();
                return sc.Result;
            }

            return null;
        });
    }

    private void Close()
    {
        if (!_disposed && client.IsConnected)
            client.Disconnect();
    }

    public void Dispose()
    {
        client.Dispose();

        _disposed = true;
    }

    public bool IsConnected => !_disposed && client.IsConnected;
}