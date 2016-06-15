using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCFRequester01.ServiceReference1;


namespace WCFRequester01
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new ServiceReference1.Service1Client();
            string returnedData = client.GetData(1);

            var ct = new ServiceReference1.CompositeType();
            ct.BoolValue = true;
            ct.StringValue = "Input value";
            var returnedcompositeval = client.GetDataUsingDataContract(ct);

            Console.WriteLine(returnedData);
            Console.WriteLine(returnedcompositeval.StringValue);
            Console.ReadLine();
        }
    }
}
