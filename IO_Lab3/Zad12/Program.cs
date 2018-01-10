using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zad12
{

    class Program
    {
        public Task<TResultDataStructure> OperationTask(byte[] buffer)
        {
            TaskCompletionSource<TResultDataStructure> tcs = new TaskCompletionSource<TResultDataStructure>();
            Task.Run(() =>
            {
                tcs.SetResult(new TResultDataStructure(31, 126806));
            });
            return tcs.Task;
        }
        static void Main(string[] args)
        {

        }
    }
}
