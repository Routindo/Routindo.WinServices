using System.Collections.Generic;
using Umator.Contract;
using Umator.Contract.Services;
using Umator.Plugins.WinServices.Components.ControlServiceAction;
using Umator.Plugins.WinServices.Components.ServiceWatcher;

namespace Umator.Plugins.WinServices.Components.Mappers
{
    [PluginItemInfo("39F74215-46D2-452E-9D0E-8AB6D5008C51", "Service Name Mapper",
        "Map arguments of " + nameof(WinServiceWatcher) + "to arguments of " + nameof(ControlServiceAction))]
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
