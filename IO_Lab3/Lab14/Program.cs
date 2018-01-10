using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Zad14
{
    class Program
    {
        public static async Task<XmlDocument> Zadanie3(string address)
        {
            WebClient webClient = new WebClient();
            string xmlContent = await webClient.DownloadStringTaskAsync(new Uri(address));
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlContent);
            return doc;
        }
        static void Main(string[] args)
        {
            //var task =Zadanie3("https://www.google.pl/");
            Console.WriteLine(Zadanie3("http://www.feedforall.com/sample.xml").Result.InnerText);
            //task.Wait();
            //var RSS = task.GetAwaiter().GetResult();
            
            Console.WriteLine();
        }
    }
}
