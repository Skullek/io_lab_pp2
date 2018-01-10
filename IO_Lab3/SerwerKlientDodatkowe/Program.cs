using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



namespace SerwerKlientDodatkowe
{
    class Server
    {
        TcpListener server;
        int port;
        IPAddress address;
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        Task serverTask;

        public Server()
        {
            address = IPAddress.Any;
            port = 2048;
        }

        public async Task RunAsync(CancellationToken ct)
        {

            server = new TcpListener(address, port);

            try
            {
                server.Start();

            }
            catch (SocketException ex)
            {
                throw (ex);
            }
            while (true)
            {
                TcpClient tcpClient = await server.AcceptTcpClientAsync();
                byte[] buffer = new byte[1024];
                await tcpClient.GetStream().ReadAsync(buffer, 0, buffer.Length).ContinueWith(
                    async (x) =>
                    {
                        int i = x.Result;
                        while (true)
                        {
                            tcpClient.GetStream().WriteAsync(buffer, 0, i);
                            i = await tcpClient.GetStream().ReadAsync(buffer, 0, buffer.Length);
                            Console.WriteLine(Encoding.ASCII.GetString(buffer, 0, i));
                        }
                    });
            }
        }

        public void Run()
        {
            Console.WriteLine("Server - start");
            serverTask = RunAsync(cancellationTokenSource.Token);
        }
        public void Stop()
        {
            cancellationTokenSource.Cancel();
            server.Stop();
            Console.WriteLine("Server - stop");
        }

    }
    class Client
    {
        private TcpClient tcpClient;
        //
        public void Connect()
        {
            tcpClient = new TcpClient();
            tcpClient.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));
        }

        public async Task<string> Ping(string message)
        {
            byte[] buffer = new ASCIIEncoding().GetBytes(message);
            await tcpClient.GetStream().WriteAsync(buffer, 0, buffer.Length);
            buffer = new byte[1024];
            Thread.Sleep(1000);
            var t = await tcpClient.GetStream().ReadAsync(buffer, 0, buffer.Length);
            Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, t));
            return Encoding.UTF8.GetString(buffer, 0, t);
        }
        public async Task<IEnumerable<string>> KeepPinging(string message, CancellationToken token)
        {
            List<string> messages = new List<string>();
            bool done = false;
            while (!done)
            {
                if (token.IsCancellationRequested)
                    done = true;
                messages.Add(await Ping(message));
            }
            return messages;
        }

        static void Main(string[] args)
        {
            Server s = new Server();
            s.Run();
            Client c1 = new Client();
            Client c2 = new Client();
            Client c3 = new Client();
            c1.Connect();
            c2.Connect();
            c3.Connect();

            CancellationTokenSource ct1 = new CancellationTokenSource();
            CancellationTokenSource ct2 = new CancellationTokenSource();
            CancellationTokenSource ct3 = new CancellationTokenSource();

            var t1 = c1.KeepPinging("Hejka",ct1.Token);
            var t2 = c2.KeepPinging("Test",ct2.Token);
            var t3 = c3.KeepPinging("3",ct3.Token);

            var tasks = new List<Task<IEnumerable<string>>>();

            tasks.Add(t1);
            tasks.Add(t2);
            tasks.Add(t3);

            ct1.CancelAfter(2000);
            ct2.CancelAfter(3000);
            ct3.CancelAfter(4000);
            Task.WaitAll(tasks.ToArray());
            s.Stop();
            Console.WriteLine("koniec main");
        }
    }


}

