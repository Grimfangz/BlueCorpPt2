using Renci.SshNet;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace TransformAndSend{
    public class SftpUploader
    {
        private readonly string _host;
        private readonly int _port;
        private readonly string _username;
        private readonly string _privateKeyPath; // path to .pem or .ppk file
        private readonly string _remoteFolder;

        public SftpUploader(IConfiguration config)
        {
            _host = config["SFTP_HOST"];
            _port = int.Parse(config["SFTP_PORT"] ?? "22");
            _username = config["SFTP_USERNAME"];
            _privateKeyPath = config["SFTP_PRIVATE_KEY_PATH"];
            _remoteFolder = config["SFTP_UPLOAD_FOLDER"];
        }

        public void UploadCsv(string fileName, string csvContent)
        {
            using var keyFile = new PrivateKeyFile(_privateKeyPath);
            var keyFiles = new[] { keyFile };
            using var client = new SftpClient(_host, _port, _username, keyFiles);

            client.Connect();

            if (!client.IsConnected)
                throw new Exception("Could not connect to SFTP server");

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));
            var remotePath = $"{_remoteFolder}/{fileName}";
            client.UploadFile(stream, remotePath, true);

            client.Disconnect();
        }
    }
}

