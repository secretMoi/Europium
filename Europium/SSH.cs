using Renci.SshNet;

namespace Europium;

public class SSH
{
    private string _user;
    private string _password;
    private string _host;
    private int _port;

    private bool _disposed; // permet de savoir si la connexion a été disposed

    private readonly SshClient client; // contient la connexion
    private static readonly Mutex VerrouMutex = new Mutex();

    // events EventHandler<T>
    public event EventHandler<CommandEventArgs> CommandeEvent;
    public event EventHandler<CommandEventArgs> ConnectEvent;
    public event EventHandler<CommandEventArgs> DisconnectEvent;

    public SSH(string host, string user, string password, int port = 22)
    {
        _user = user;
        _password = password;
        _host = host;
        _port = port;

        client = new SshClient(host, port, user, password);

        _disposed = false;
    }

    public void Connect(EventHandler<CommandEventArgs> resultat = null)
    {
        if (resultat != null)
            ConnectEvent += resultat;

        var threadConnect = new Thread(ThreadConnect);
        threadConnect.Start();
    }

    private void ThreadConnect()
    {
        if(!_disposed)
            if (client.IsConnected) return;

        VerrouMutex.WaitOne();

        try
        {
            client.Connect();
            LaunchEvent(new CommandEventArgs(), ConnectEvent);
        }
        catch
        {
        }

        VerrouMutex.ReleaseMutex();
    }
    
    public async Task ConnectAsync()
    {
        if(!_disposed)
            if (client.IsConnected) return;

        try
        {
            await Task.Run(() => client.Connect());
        }
        catch
        {
        }
    }

    public void Disconnect(EventHandler<CommandEventArgs> resultat = null)
    {
        if (resultat != null)
            DisconnectEvent += resultat;

        var threadClose = new Thread(Close);
        threadClose.Start();
    }

    /*
     * commande, commande à exécuter
     * resultat, fonction de retour à appeler
     * id, donne un id si plusieurs commandes sont appelées en même temps, permettant de les différencier
     * keepSubscriber, indique si le subsciber veut se désabonner après le résultat
     */
    public void SendCommand(string commande, EventHandler<CommandEventArgs> resultat, int id, bool keepSubscriber)
    {
        if(resultat != null)
            CommandeEvent += resultat;

        var t = new Thread(() => RunCommand(resultat, commande, id, keepSubscriber));
        t.Start();
    }

    public void SendCommand(string commande, EventHandler<CommandEventArgs> resultat, bool keepSubscriber)
    {
        SendCommand(commande, resultat, 0, keepSubscriber);
    }

    public void SendCommand(string commande, EventHandler<CommandEventArgs> resultat, int id)
    {
        SendCommand(commande, resultat, id, false);
    }

    public void SendCommand(string commande, EventHandler<CommandEventArgs> resultat)
    {
        SendCommand(commande, resultat, 0, false);
    }

    // todo uliser mutex différent pour avoir plusieurs commandes en même temps
    private void RunCommand(EventHandler<CommandEventArgs> fonctionRetour, string commande, int id, bool keepSubsciber)
    {
        VerrouMutex.WaitOne();

        if (client != null && !_disposed) // si la connexion n'a pas été détruite entre temps
        {
            if (client.IsConnected) // si on a pas été déconnecté entre temps
            {
                SshCommand sc = client.CreateCommand(commande);
                sc.Execute();
                string resultat = sc.Result;

                // lance l'event de fin de commande pour notifier les subscibers du resultat
                LaunchEvent(new CommandEventArgs(resultat, id, keepSubsciber), CommandeEvent);

                // si les subscibers ne veulent recevoir le résultat qu'une fois
                if (!keepSubsciber)
                    CommandeEvent -= fonctionRetour;
            }
        }
        
        VerrouMutex.ReleaseMutex();
    }

    public async Task<String> RunCommandAsync(string command)
    {
        return await Task.Run(() =>
        {
            if (client != null && !_disposed) // si la connexion n'a pas été détruite entre temps
            {
                if (client.IsConnected) // si on a pas été déconnecté entre temps
                {
                    SshCommand sc = client.CreateCommand(command);
                    sc.Execute();
                    return sc.Result;
                }
            }

            return null;
        });
    }

    private void Close()
    {
        VerrouMutex.WaitOne();

        if (!_disposed && client.IsConnected)
            client.Disconnect();

        LaunchEvent(new CommandEventArgs(), DisconnectEvent);

        VerrouMutex.ReleaseMutex();
    }

    public void Dispose()
    {
        client.Dispose();

        _disposed = true;
    }

    // permet de lancer un event aux subsciber
    private void LaunchEvent(CommandEventArgs e, EventHandler<CommandEventArgs> handler)
    {
        handler?.Invoke(this, e);
    }

    public bool IsConnected => !_disposed && client.IsConnected;

    public void ClearEvents()
    {
        ConnectEvent = null;
        CommandeEvent = null;
        DisconnectEvent = null;
    }
}

// class qui permet de transmettre les arguments
public class CommandEventArgs : EventArgs
{
    public CommandEventArgs(string s, int id, bool keepSubsciber)
    {
        Message = s;
        Id = id;
        KeepSubsciber = keepSubsciber;
    }

    public CommandEventArgs()
    {
    }

    public int Id { get; set; }

    public string Message { get; set; }

    public bool KeepSubsciber { get; set; }
}
