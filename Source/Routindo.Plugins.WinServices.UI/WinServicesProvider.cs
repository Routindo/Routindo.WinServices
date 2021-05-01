using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using Routindo.Plugins.WinServices.UI.Models;

namespace Routindo.Plugins.WinServices.UI
{
    public class WinServicesProvider
    {
        private static WinServicesProvider instance;
        private static readonly object syncObj = new object();

        private List<WinServiceModel> services = null; 

        static WinServicesProvider()
        {
            if(instance != null)
                return;

            lock (syncObj)
            {
                if(instance != null)
                    return;

                instance = new WinServicesProvider();
            }
        }

        public static List<WinServiceModel> GetServices()
        {
            if (instance.services == null)
                instance.services = ServiceController.GetServices()
                    .Select(e => new WinServiceModel(e.ServiceName, e.DisplayName)).OrderBy(e=> e.DisplayName).ToList();

            return instance.services;
        }
    }
}
