using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Web;
using WCFWebService;

namespace ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ServiceHost host = new ServiceHost(typeof(FlightTickets));
                host.Open();
                WebServiceHost restHost = new WebServiceHost(typeof(FlightTicketsRest));
                restHost.Open();
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                host.Close();
                restHost.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }
    }
}
