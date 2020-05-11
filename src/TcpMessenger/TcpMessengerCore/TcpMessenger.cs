using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TcpMessengerCore
{
    public class TcpMessenger
    {
        private TcpClient _tcpClient;
        private string closeCommand;

        public TcpMessenger(string host, int port, string closeCommand)
        {
            _tcpClient = new TcpClient(host, port);
            this.closeCommand = closeCommand;
        }


        public void StartDiscussion()
        {
            using (var connectionSteam = _tcpClient.GetStream())
            {
                while (true)
                {
                    ReadFromStream(connectionSteam);

                    var inputCommand = Console.ReadLine();
                    if (inputCommand == closeCommand)
                        break;
                    if (inputCommand == "c")
                        continue;
                    var messageToServer = Encoding.ASCII.GetBytes($"{inputCommand}\n");
                    connectionSteam.Write(messageToServer);
                }
            }
        }

        private void ReadFromStream(NetworkStream stream)
        {
            Thread.Sleep(500);
            var allMessage = new List<byte>();
            var allReadBytesCount = 0;

            while (stream.DataAvailable)
            {
                var inputBytes = new byte[1024];
                var b = stream.Read(inputBytes);
                allMessage.AddRange(inputBytes.Take(b));
                allReadBytesCount += b;
            }

            Console.WriteLine(Encoding.UTF8.GetString(allMessage.ToArray(), 0, allReadBytesCount));
        }
    }
}