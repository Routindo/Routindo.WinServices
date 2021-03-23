using Routindo.Contract;
using Routindo.Contract.Attributes;
using Routindo.Plugins.WinServices.Components.ControlServiceAction;
using Routindo.Plugins.WinServices.Components.ServiceWatcher;
using Routindo.Plugins.WinServices.UI.Views;

[assembly: ComponentConfigurator(typeof(WinServiceActionConfigurator), WinServiceAction.ComponentUniqueId, "Configurator for Windows Service Status Controller action")]
[assembly: ComponentConfigurator(typeof(WinServiceWatcherConfigurator), WinServiceWatcher.ComponentUniqueId, "Configurator for Windows Service Status Watcher")]