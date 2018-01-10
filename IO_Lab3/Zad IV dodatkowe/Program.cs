using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZadIVdodatkowe
{
    class Server
    {
        bool isListening;
        TcpListener server;
        int port;
        IPAddress address;
        AutoResetEvent AutoResetEvent;

        public Server(AutoResetEvent autoResetEvent)
        {
            isListening = false;
            AutoResetEvent = autoResetEvent;
            address = IPAddress.Any;
            port = 2048;
            server = new TcpListener(address, port);
            ThreadPool.QueueUserWorkItem(ServerListener, server);
        }

        public void Run()
        {
            server.Start();
            Console.WriteLine("Server - start");
            isListening = true;
        }
        public void Stop()
        {
            server.Stop();
            isListening = false;
            AutoResetEvent.Set();
            Console.WriteLine("Server - stop");
        }

        public void ServerListener(object state)
        {
            var server = (TcpListener)state;

            while (isListening)
            {
                TcpClient client = server.AcceptTcpClient();
                ThreadPool.QueueUserWorkItem(Connected, client);
            }
        }

        public void Connected(object state)
        {
            var client = (TcpClient)state;
            String message = "";
            while (client.Connected)
            {
                var requestBytes = new byte[1024];
                client.GetStream().Read(requestBytes, 0, requestBytes.Length);
                var request = Encoding.ASCII.GetString(requestBytes.Take(3).ToArray());
                if (request == "HEY")
                {
                    client.GetStream().Write(Encoding.ASCII.GetBytes("HEY"), 0, "HEY".Length);
                }
                else if (request == "BYE")
                {
                    client.GetStream().Write(Encoding.ASCII.GetBytes("BYE"), 0, "BYE".Length);
                    Console.WriteLine(message);
                }
                else
                {
                    message += Encoding.ASCII.GetString(requestBytes).Trim('\0');
                    client.GetStream().Write(Encoding.ASCII.GetBytes("ACK"), 0, "ACK".Length);
                }
            }
        }
    }

    class Client
    {
        private TcpClient client;
        private byte[] buffer;

        public Client()
        {
            buffer = new byte[3];
            client = new TcpClient("127.0.0.1", 2048);
            client.GetStream().ReadTimeout = 4000;
            client.GetStream().Write(Encoding.ASCII.GetBytes("HEY"), 0, "HEY".Length);
            client.GetStream().Read(buffer, 0, buffer.Length);
            Console.WriteLine(Encoding.ASCII.GetString(buffer));
        }


        public void SendMessage(string message)
        {
            client.GetStream().Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
            client.GetStream().Read(buffer, 0, buffer.Length);
            Console.WriteLine(Encoding.ASCII.GetString(buffer));
        }

        public void SendBye()
        {
            client.GetStream().Write(Encoding.ASCII.GetBytes("BYE"), 0, "BYE".Length);
            var asyncResult = client.GetStream().BeginRead(buffer, 0, buffer.Length, CloseConnection, null);
            client.GetStream().EndRead(asyncResult);

        }
        public void CloseConnection(object sender)
        {
            var response = Encoding.ASCII.GetString(buffer);
            Console.WriteLine(response);
            client.Close();
        }
    }

    class program
    {
        static void Main(string[] args)
        {
            var AutoResetEvent = new AutoResetEvent(false);
            var server = new Server(AutoResetEvent);
            server.Run();    
            var client1 = new Client();
            var client2 = new Client();
            var client3 = new Client();
            client1.SendMessage("Czesc1 ");
            client3.SendMessage("Czesc3 ");
            client1.SendMessage("Wiadomosc od 1");
            client3.SendMessage("Wiadomosc od 3");
            client2.SendMessage("klient2");
            client1.SendBye();
            client2.SendMessage("Klient nr2");
            client2.SendBye();
            client3.SendBye();
            server.Stop();
            AutoResetEvent.WaitOne();

        }
    }
}