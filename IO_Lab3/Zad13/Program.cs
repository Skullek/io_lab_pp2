using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zad13
{
    //public void Zadanie2() // async dla await
    //{
    //    //ZADANIE 2. ODKOMENTUJ I POPRAW  
    //    /*
    //        Task.Run( // dodanie await
    //            () == // zła lambda
    //            {
    //               Z2 = true;
    //            }
    //     */
    //}

    class Program
    {
        public Boolean Z2 { get; set; } = true;
        public async void Zadanie2()
        {
            await Task.Run(
             () =>
             {
                 Z2 = true;
             });
        }
        static void Main(string[] args)
        {
        }
    }
}
