using Umator.Contract;
using Umator.Plugins.WinServices.Components.ControlServiceAction;
using Umator.Plugins.WinServices.Components.ServiceWatcher;
using Umator.Plugins.WinServices.UI.Views;

[assembly: ComponentConfigurator(typeof(WinServiceActionConfigurator), WinServiceAction.ComponentUniqueId, "Configurator for Windows Service Status Controller action")]
[assembly: ComponentConfigurator(typeof(WinServiceWatcherConfigurator), WinServiceWatcher.ComponentUniqueId, "Configurator for Windows Service Status Watcher")]