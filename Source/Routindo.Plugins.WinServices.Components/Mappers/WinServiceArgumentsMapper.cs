﻿using System.Collections.Generic;
using Routindo.Contract;
using Routindo.Contract.Arguments;
using Routindo.Contract.Attributes;
using Routindo.Contract.Services;
using Routindo.Plugins.WinServices.Components.ControlServiceAction;
using Routindo.Plugins.WinServices.Components.ServiceWatcher;

namespace Routindo.Plugins.WinServices.Components.Mappers
{
    [PluginItemInfo("39F74215-46D2-452E-9D0E-8AB6D5008C51", nameof(WinServiceArgumentsMapper),
        "Map arguments of " + nameof(WinServiceWatcher) + "to arguments of " + nameof(ControlServiceAction), Category = "Windows Services", FriendlyName = "Service Watcher / Action Arguments Mapper")]
    public class WinServiceArgumentsMapper: IArgumentsMapper
    {
        public string Id { get; set; }
        public ILoggingService LoggingService { get; set; }

        public ArgumentCollection Map(ArgumentCollection arguments)
        {
            if (!arguments.HasArgument(WinServiceWatcherResultArgs.ServiceName) ||
                !(arguments[WinServiceWatcherResultArgs.ServiceName] is string))
                return null;

            return ArgumentCollection.New()
                .WithArgument(WinServiceActionExecutionArgs.ServiceName,
                    arguments.GetValue<string>(WinServiceWatcherResultArgs.ServiceName));
        }
    }
}
