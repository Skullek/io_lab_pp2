using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IO_2_8
{
    class Program
    {
        public delegate int del(int x);
        static del SilniaRR, SilniaII, fibRR, fibII;
        static int silniaR(int i)
        {
            if (i < 1)
                return 1;
            else
                return i * silniaR(i - 1);
        }
        static int silniaI(int n)
        {
            int result = 1;
            for (int i = 1; i <= n; i++)
            {
                result *= i;
            }
            return result;
        }
        static int fibR(int n)
        {
            if ((n == 1) || (n == 2))
                return 1;
            else
                return fibR(n - 1) + fibR(n - 2);
        }
         static int fibI(int n)
        {
            int a = 0;
            int b = 1;
            for (int i = 0; i < n; i++)
            {
                int temp = a;
                a = b;
                b = temp + b;
            }
            return a;
        }


        static void Main(string[] args)
        {
            SilniaRR = new del(silniaR);
            SilniaII = new del(silniaI);
            fibRR = new del(fibR);
            fibII = new del(fibI);

            IAsyncResult sRR = SilniaRR.BeginInvoke(10, null, null);
            IAsyncResult sII = SilniaII.BeginInvoke(30, null, null);        
            IAsyncResult fRR = fibRR.BeginInvoke(10, null, null);       
            IAsyncResult fII = fibII.BeginInvoke(10, null, null);
            int result1 = SilniaRR.EndInvoke(sRR);
            int result2 = SilniaII.EndInvoke(sII);
            int result3 = fibRR.EndInvoke(fRR);
            int result4 = fibII.EndInvoke(fII);

            Console.WriteLine("SilniaR: " + result1 + "\nSilniaI: " + result2 + "\nFibR: " + result3 + "\nFibI:" + result4);



        }
    }
}
