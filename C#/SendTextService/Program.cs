
using System.ServiceProcess;

namespace SendTextService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            #if DEBUG
            var myService = new SendTextService();
            myService.OnDebug();
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
            #else
            var servicesToRun = new ServiceBase[]
            {
                new SendTextService(), 
            };
            ServiceBase.Run(servicesToRun); 
            #endif
        }
    }
}
