using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IO_Lab3
{
    class Program
    {

        public static async Task OperationTask(object data)
        {
            Console.WriteLine("begin task");
            await Task.Run(() =>
            {
                Console.WriteLine("begin async");
                Thread.Sleep(100);
                Console.WriteLine("end async");
            });
            Console.WriteLine("End task");
        }
        static void Main(string[] args)
        {
            int test = 0;
            byte[] buffer = new byte[128];
            Console.WriteLine("begin main");
            Task task = OperationTask(buffer);
            Thread.Sleep(test);
            Console.WriteLine("program main");
            task.Wait();
            Console.WriteLine("end main");
        }
    }
}
