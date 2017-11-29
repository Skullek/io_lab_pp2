using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IO_2_7
{
    class Program
    {
        static void Main(string[] args)
        {
                String path = @"C:\Users\student\source\repos\IO_2\IO_2\plik.txt";
                FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
                byte[] buffer = new byte[file.Length];
                WaitHandle handle = new AutoResetEvent(false);
                IAsyncResult iar;
                iar = file.BeginRead(buffer, 0, (int)file.Length, null, null);
                file.EndRead(iar);
                Console.WriteLine(Encoding.ASCII.GetString(buffer));
                Console.ReadLine();
        }
    }
}

