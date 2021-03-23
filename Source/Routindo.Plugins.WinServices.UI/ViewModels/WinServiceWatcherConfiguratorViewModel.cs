using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceProcess;
using System.Windows.Input;
using Routindo.Contract;
using Routindo.Contract.Arguments;
using Routindo.Contract.UI;
using Routindo.Plugins.WinServices.Components;
using Routindo.Plugins.WinServices.Components.ServiceWatcher;

namespace Routindo.Plugins.WinServices.UI.ViewModels
{
    public class WinServiceWatcherConfiguratorViewModel : PluginConfiguratorViewModelBase
    {
        private string _serviceName;
        private ServiceControllerStatus? _watchStatus;
        private bool _anyStatus = true;

        public WinServiceWatcherConfiguratorViewModel()
        {
            var statuses = Enum.GetValues<ServiceControllerStatus>().ToList();
            this.Statuses = new ObservableCollection<ServiceControllerStatus>(statuses);
            this.Services = new ObservableCollection<string>(WinServiceUtilities.GetServices());
        }

        
        public string ServiceName
        {
            get => _serviceName;
            set
            {
                _serviceName = value;
                ClearPropertyErrors();
                ValidateNonNullOrEmptyString(ServiceName);
                OnPropertyChanged();
            }
        }

        public ServiceControllerStatus? WatchStatus
        {
            get => _watchStatus;
            set
            {
                _watchStatus = value;
                ClearPropertyErrors();
                ValidateServiceStatus();
                OnPropertyChanged();
            }
        }

        public bool AnyStatus
        {
            get => _anyStatus;
            set
            {
                _anyStatus = value;
                OnPropertyChanged();
                WatchStatus = null;
            }
        }

        public ObservableCollection<ServiceControllerStatus> Statuses { get; set; }
        public ObservableCollection<string> Services { get; set; }

        public override void Configure()
        {
            this.InstanceArguments = ArgumentCollection.New()
                .WithArgument(WinServiceWatcherInstanceArgs.ServiceName, ServiceName)
                .WithArgument(WinServiceWatcherInstanceArgs.WatchAnyStatus, AnyStatus);
            if (WatchStatus.HasValue)
                InstanceArguments.Add(WinServiceWatcherInstanceArgs.WatchStatus, (int) WatchStatus);
        }

        public override void SetArguments(ArgumentCollection arguments)
        {
            if (arguments == null || !arguments.Any())
                return;

            if (arguments.HasArgument(WinServiceWatcherInstanceArgs.ServiceName))
                ServiceName = arguments.GetValue<string>(WinServiceWatcherInstanceArgs.ServiceName);

            if (arguments.HasArgument(WinServiceWatcherInstanceArgs.WatchAnyStatus))
                AnyStatus = arguments.GetValue<bool>(WinServiceWatcherInstanceArgs.WatchAnyStatus);

            if (arguments.HasArgument(WinServiceWatcherInstanceArgs.WatchStatus))
            {
                int statusId  = arguments.GetValue<int>(WinServiceWatcherInstanceArgs.WatchStatus);
                if (Enum.IsDefined(typeof(ServiceControllerStatus), statusId))
                {
                    WatchStatus = (ServiceControllerStatus) statusId;
                }
            }
        }

        private void ValidateServiceStatus()
        {
            if (!WatchStatus.HasValue && !AnyStatus)
            {
                AddPropertyError(nameof(WatchStatus), "If option 'Any Status' is not selected, It's mandatory to choose a status from the list");
            }
        }

        protected override void ValidateProperties()
        {
            base.ValidateProperties();

            // Service name
            ClearPropertyErrors(nameof(ServiceName));
            ValidateNonNullOrEmptyString(ServiceName, nameof(ServiceName));
            OnPropertyChanged(nameof(ServiceName));

            // Watch Status
            ClearPropertyErrors(nameof(WatchStatus));
            ValidateServiceStatus();
            OnPropertyChanged(nameof(WatchStatus));
        }
    }
}
