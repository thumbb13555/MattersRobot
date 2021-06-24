using MattersAutoClap._Module.Entity;
using MattersRobot.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace MattersRobot
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        const bool isDegugging = true;
        static void Main()
        {

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]{new MainService()};
            if (Environment.UserInteractive)
            {
                RunInteractive(ServicesToRun);
            }
            else
            {
                ServiceBase.Run(ServicesToRun);
            }

        }
        public static void RunInteractive(ServiceBase[] services)
        {
            Console.WriteLine();
            Console.WriteLine("Install the services in interactive mode.");
            Console.WriteLine();
            // Get the method to invoke on each service to start it
            var onStartMethod =
                typeof(ServiceBase).GetMethod("OnStart", BindingFlags.Instance | BindingFlags.NonPublic);
            // Start services loop
            foreach (var service in services)
            {
                Console.WriteLine("Installing {0} ... ", service.ServiceName);
                onStartMethod.Invoke(service, new object[] { new string[] { } });
                Console.WriteLine("Installed {0} ", service.ServiceName);
                Console.WriteLine();
            }
            // Waiting the end
            Console.WriteLine("Press a key to uninstall all services...");
            Console.ReadKey();
            Console.WriteLine();
            // Get the method to invoke on each service to stop it
            var onStopMethod = typeof(ServiceBase).GetMethod("OnStop", BindingFlags.Instance | BindingFlags.NonPublic);
            // Stop loop
            foreach (var service in services)
            {
                Console.Write("Uninstalling {0} ... ", service.ServiceName);
                onStopMethod.Invoke(service, null);
                Console.WriteLine("Uninstalled {0}", service.ServiceName);
            }
            Console.WriteLine();
            Console.WriteLine("All services are uninstalled.");
            // Waiting a key press to not return to VS directly
            if (Debugger.IsAttached)
            {
                Console.WriteLine();
                Console.Write("=== Press a key to quit ===");
                Console.ReadKey();
            }
        }
    }
}
