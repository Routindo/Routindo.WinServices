﻿using System;
using System.ServiceProcess;
using NLog;
using Umator.Contract;

namespace Umator.Plugins.WinServices.Components.ServiceWatcher
{
    [PluginItemInfo(ComponentUniqueId, "Windows Service Watcher",
         "Watch a windows service status and notifies when Service is on a specific status"),
     ResultArgumentsClass(typeof(WinServiceWatcherResultArgs))]

    public class WinServiceWatcher: IWatcher
    {
        public const string ComponentUniqueId = "7C211B6E-02E6-4979-AABE-909B770AAA4D";
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public string Id { get; set; }

        [Argument(WinServiceWatcherInstanceArgs.ServiceName)] public string ServiceName { get; set; }
        [Argument(WinServiceWatcherInstanceArgs.WatchStatus)] public int Status { get; set; }
        [Argument(WinServiceWatcherInstanceArgs.WatchAnyStatus)] public bool AnyStatus { get; set; }

        private ServiceControllerStatus? _serviceStatus; 

        public WatcherResult Watch()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ServiceName)) 
                    throw new Exception("ServiceName is incorrect ! NULL or EMPTY");

                var status = WinServiceUtilities.GetServiceStatus(ServiceName);

                bool notify = false;

                if (!_serviceStatus.HasValue || status != _serviceStatus.Value)
                {
                    _serviceStatus = status;
                    if (AnyStatus)
                        notify = true;
                    else if((int)status == Status)
                    {
                        notify = true;
                    }
                }

                if (!notify) return WatcherResult.NotFound;
                _serviceStatus = status;
                return WatcherResult.Succeed(ArgumentCollection.New()
                    .WithArgument(WinServiceWatcherResultArgs.ServiceName, ServiceName)
                    .WithArgument(WinServiceWatcherResultArgs.Status, status.ToString("G")));

            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return WatcherResult.NotFound
                    .WithException(exception);
            }
        }
    }
}