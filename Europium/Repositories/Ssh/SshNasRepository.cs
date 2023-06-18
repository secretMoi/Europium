using Europium.Dtos;
using Europium.Models;
using Microsoft.Extensions.Options;
using Renci.SshNet;

namespace Europium.Repositories.Ssh;

public class SshNasRepository
{
	private bool _disposed; // permet de savoir si la connexion a été disposed
    
        private static SshClient? _client; // contient la connexion
        private static bool _isConnecting;
        
        private bool IsConnected => !_disposed && _client is not null && _client.IsConnected;
        
        public SshNasRepository(IOptions<AppConfig> optionsSnapshot)
        {
            if(_client is null)
            {
                var appConfig = optionsSnapshot.Value;
                _client = new SshClient(appConfig.SshHost, appConfig.SshPort, appConfig.SshUser, appConfig.SshPassword);
                _client.ConnectionInfo.Timeout = TimeSpan.FromSeconds(5);
            }
    
            _disposed = false;
        }
    
        public async Task ConnectAsync()
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

        public async Task<string?> RunCommandAsync(string command)
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
        
        public string GetSshCommandToExecute(string path, FileType fileType, int limit)
        {
            var commandToExecute = "find " + path;

            if (fileType == FileType.File)
                commandToExecute += " -type f";
            if (fileType == FileType.Folder)
                commandToExecute += " -type d";

            commandToExecute += " -exec du -S {} + | sort -rh | head -n " + limit;

            return commandToExecute;
        }
}