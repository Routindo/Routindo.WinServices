using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceProcess;
using System.Windows.Input;
using Routindo.Contract.Arguments;
using Routindo.Contract.UI;
using Routindo.Plugins.WinServices.Components;
using Routindo.Plugins.WinServices.Components.ControlServiceAction;
using Routindo.Plugins.WinServices.UI.Models;

namespace Routindo.Plugins.WinServices.UI.ViewModels
{
    public class WinServiceActionConfiguratorViewModel: PluginConfiguratorViewModelBase
    {
        private bool _wait = true;
        private ServiceTargetStatus? _status;
        private WinServiceModel _selectedService;
        private int _waitTimeout = 10;

        public WinServiceActionConfiguratorViewModel()
        {
            var statuses = Enum.GetValues<ServiceTargetStatus>().ToList();
            this.Statuses = new ObservableCollection<ServiceTargetStatus>(statuses);
            this.Services = new ObservableCollection<WinServiceModel>(WinServicesProvider.GetServices());
            ResetSelectedServiceCommand = new RelayCommand(ResetSelectedService);
        }

        public ICommand ResetSelectedServiceCommand { get; }

        private void ResetSelectedService()
        {
            this.SelectedService = null;
        }

        public ObservableCollection<ServiceTargetStatus> Statuses { get; set; }
        public ObservableCollection<WinServiceModel> Services { get; set; }

        public WinServiceModel SelectedService
        {
            get => _selectedService;
            set
            {
                _selectedService = value;
                OnPropertyChanged();
            }
        }

        public ServiceTargetStatus? Status
        {
            get => _status;
            set
            {
                _status = value;
                ClearPropertyErrors();
                ValidateServiceStatus();
                OnPropertyChanged();
            }
        }

        public bool Wait
        {
            get => _wait;
            set
            {
                _wait = value;
                OnPropertyChanged();
                WaitTimeout = 0;
            }
        }

        public int WaitTimeout
        {
            get => _waitTimeout;
            set
            {
                _waitTimeout = value;
                ClearPropertyErrors();
                if (Wait)
                    ValidateNumber(value, i => i > 0);
                OnPropertyChanged();
            }
        }

        public override void Configure()
        {
            this.InstanceArguments = ArgumentCollection.New()
                .WithArgument(WinServiceActionInstanceArgs.ServiceName, SelectedService?.ServiceName)
                .WithArgument(WinServiceActionInstanceArgs.Status, (int)Status)
                .WithArgument(WinServiceActionInstanceArgs.Wait, Wait)
                .WithArgument(WinServiceActionInstanceArgs.WaitTimeout, WaitTimeout);
        }

        public override void SetArguments(ArgumentCollection arguments)
        {
            if (arguments == null || !arguments.Any())
                return;

            if (arguments.HasArgument(WinServiceActionInstanceArgs.ServiceName))
            {
                var serviceName = arguments.GetValue<string>(WinServiceActionInstanceArgs.ServiceName);
                if (!string.IsNullOrWhiteSpace(serviceName))
                    SelectedService = WinServicesProvider.GetServices().SingleOrDefault(e=> e.ServiceName == serviceName);
            }

            if (arguments.HasArgument(WinServiceActionInstanceArgs.Wait))
                Wait = arguments.GetValue<bool>(WinServiceActionInstanceArgs.Wait);

            if (arguments.HasArgument(WinServiceActionInstanceArgs.WaitTimeout))
                WaitTimeout = arguments.GetValue<int>(WinServiceActionInstanceArgs.WaitTimeout);

            if (arguments.HasArgument(WinServiceActionInstanceArgs.Status))
            {
                int statusId = arguments.GetValue<int>(WinServiceActionInstanceArgs.Status);
                if (Enum.IsDefined(typeof(ServiceTargetStatus), statusId))
                {
                    Status = (ServiceTargetStatus)statusId;
                }
            }
        }

        private void ValidateServiceStatus()
        {
            if (!Status.HasValue)
            {
                AddPropertyError(nameof(Status), "This field is mandatory");
            }
        }

        protected override void ValidateProperties()
        {
            base.ValidateProperties();
            // Status 
            ClearPropertyErrors(nameof(Status));
            ValidateServiceStatus();
            OnPropertyChanged(nameof(Status));

            // Wait Timeout 
            ClearPropertyErrors(nameof(WaitTimeout));
            if (Wait)
                ValidateNumber(WaitTimeout, i => i > 0);
            OnPropertyChanged(nameof(WaitTimeout));
        }
    }
}
