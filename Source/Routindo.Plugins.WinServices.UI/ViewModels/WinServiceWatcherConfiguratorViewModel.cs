using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceProcess;
using Routindo.Contract.Arguments;
using Routindo.Contract.UI;
using Routindo.Plugins.WinServices.Components.ServiceWatcher;
using Routindo.Plugins.WinServices.UI.Models;

namespace Routindo.Plugins.WinServices.UI.ViewModels
{
    public class WinServiceWatcherConfiguratorViewModel : PluginConfiguratorViewModelBase
    {
        private WinServiceModel _selectedService;
        private ServiceControllerStatus? _watchStatus;
        private bool _anyStatus = true;

        public WinServiceWatcherConfiguratorViewModel()
        {
            var statuses = Enum.GetValues<ServiceControllerStatus>().ToList();
            this.Statuses = new ObservableCollection<ServiceControllerStatus>(statuses);
            this.Services = new ObservableCollection<WinServiceModel>(WinServicesProvider.GetServices());
        }


        public WinServiceModel SelectedService
        {
            get => _selectedService;
            set
            {
                _selectedService = value;
                ClearPropertyErrors();
                if (_selectedService == null)
                {
                    AddPropertyError(nameof(SelectedService), "Mandatory Field");
                }
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
        public ObservableCollection<WinServiceModel> Services { get; set; }

        public override void Configure()
        {
            this.InstanceArguments = ArgumentCollection.New()
                .WithArgument(WinServiceWatcherInstanceArgs.ServiceName, SelectedService?.ServiceName)
                .WithArgument(WinServiceWatcherInstanceArgs.WatchAnyStatus, AnyStatus);
            if (WatchStatus.HasValue)
                InstanceArguments.Add(WinServiceWatcherInstanceArgs.WatchStatus, (int) WatchStatus);
        }

        public override void SetArguments(ArgumentCollection arguments)
        {
            if (arguments == null || !arguments.Any())
                return;

            if (arguments.HasArgument(WinServiceWatcherInstanceArgs.ServiceName))
            {
                string serviceName = arguments.GetValue<string>(WinServiceWatcherInstanceArgs.ServiceName);
                if (!string.IsNullOrWhiteSpace(serviceName))
                {
                    SelectedService = WinServicesProvider.GetServices().SingleOrDefault(e=> e.ServiceName == serviceName) ;
                }
            }

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
            ClearPropertyErrors(nameof(SelectedService));
            if (SelectedService == null)
            {
                AddPropertyError(nameof(SelectedService), "Mandatory Field");
            }
            OnPropertyChanged(nameof(SelectedService));

            // Watch Status
            ClearPropertyErrors(nameof(WatchStatus));
            ValidateServiceStatus();
            OnPropertyChanged(nameof(WatchStatus));
        }
    }
}
