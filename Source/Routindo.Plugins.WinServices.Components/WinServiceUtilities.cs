using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;

namespace Routindo.Plugins.WinServices.Components
{
    public static class WinServiceUtilities
    {
        /// <summary>
        /// Gets the service status.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <returns></returns>
        public static ServiceControllerStatus GetServiceStatus(string serviceName)
        {
            using ServiceController serviceController = new ServiceController(serviceName);
            return serviceController.Status;
        }

        /// <summary>
        /// Starts the service.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="waitForStart">if set to <c>true</c> [wait for start].</param>
        /// <param name="timeoutSeconds">The timeout seconds.</param>
        /// <returns></returns>
        public static bool StartService(string serviceName, bool waitForStart = true, int timeoutSeconds = 10)
        {
            using ServiceController serviceController = new ServiceController(serviceName);
            serviceController.Refresh();
            if (serviceController.Status != ServiceControllerStatus.Stopped)
                return false;
            serviceController.Start();
            if (!waitForStart)
                return true;

            serviceController.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(timeoutSeconds));
            serviceController.Refresh();
            return serviceController.Status == ServiceControllerStatus.Running;
        }

        /// <summary>
        /// Stops the service.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="waitForStop">if set to <c>true</c> [wait for stop].</param>
        /// <param name="timeoutSeconds">The timeout seconds.</param>
        /// <returns></returns>
        public static bool StopService(string serviceName, bool waitForStop = true, int timeoutSeconds = 10)
        {
            using ServiceController serviceController = new ServiceController(serviceName);
            if (!serviceController.CanStop)
                return false;

            serviceController.Stop();
            if (!waitForStop)
                return true;

            serviceController.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(timeoutSeconds));

            serviceController.Refresh();
            return serviceController.Status == ServiceControllerStatus.Stopped;
        }

        public static bool PauseService(string serviceName, bool wait = true, int timeoutSeconds = 10)
        {
            using ServiceController serviceController = new ServiceController(serviceName);
            if (!serviceController.CanPauseAndContinue)
                return false;

            serviceController.Pause();
            if (!wait)
                return true;

            serviceController.WaitForStatus(ServiceControllerStatus.Paused, TimeSpan.FromSeconds(timeoutSeconds));

            serviceController.Refresh();
            return serviceController.Status == ServiceControllerStatus.Paused;
        }

        public static bool ResumeService(string serviceName, bool wait = true, int timeoutSeconds = 10)
        {
            using ServiceController serviceController = new ServiceController(serviceName);
            if (!serviceController.CanPauseAndContinue)
                return false;

            serviceController.Continue();
            if (!wait)
                return true;

            serviceController.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(timeoutSeconds));

            serviceController.Refresh();
            return serviceController.Status == ServiceControllerStatus.Running;
        }

        public static List<string> GetServices()
        {
            return ServiceController.GetServices().Select(e => e.ServiceName).ToList();
        }
    }
}
