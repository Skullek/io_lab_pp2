using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IO_2
{
    class Program
    {
        static void Main(string[] args) {
            string path = @"C:\Users\student\source\repos\IO_2\IO_2\plik.txt";
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[file.Length];
            WaitHandle handle = new AutoResetEvent(false);
            file.BeginRead(buffer, 0, (int)file.Length, myAsyncCallback, new object[] {buffer, file, handle });
            handle.WaitOne();
            Console.ReadLine();
        }
        static void myAsyncCallback(IAsyncResult state)
        {
            var temp = (object[])state.AsyncState;
            var text = temp[0] as byte[];
            var file = temp[1] as FileStream;
            var handle = temp[2] as AutoResetEvent;
            Console.WriteLine(Encoding.ASCII.GetString(text));

            handle.Set();
            file.EndRead(state);

        }

    }
}
