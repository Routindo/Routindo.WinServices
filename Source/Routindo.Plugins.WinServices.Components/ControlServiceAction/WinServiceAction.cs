using System;
using Routindo.Contract;
using Routindo.Contract.Actions;
using Routindo.Contract.Arguments;
using Routindo.Contract.Attributes;
using Routindo.Contract.Services;

namespace Routindo.Plugins.WinServices.Components.ControlServiceAction
{
    [PluginItemInfo(ComponentUniqueId, "Windows Service Action",
         "Control The status of a Windows service"),
     ExecutionArgumentsClass(typeof(WinServiceActionExecutionArgs))]
    public class WinServiceAction: IAction
    {
        public const string ComponentUniqueId = "2F47FC7B-8170-4DE8-B953-41ED950380C1";

        public string Id { get; set; }
        public ILoggingService LoggingService { get; set; }

        [Argument(WinServiceActionInstanceArgs.ServiceName)] public string ServiceName { get; set; }
        [Argument(WinServiceActionInstanceArgs.Status)] public int Status { get; set; }

        [Argument(WinServiceActionInstanceArgs.Wait)] public bool Wait { get; set; } = false;

        [Argument(WinServiceActionInstanceArgs.WaitTimeout)] public int WaitTimeout { get; set; } = 10;

        public ActionResult Execute(ArgumentCollection arguments)
        {
            try
            {
                string serviceName = null;

                if (arguments.HasArgument(WinServiceActionExecutionArgs.ServiceName))
                    serviceName = arguments.GetValue<string>(WinServiceActionExecutionArgs.ServiceName);

                if (serviceName == null && !string.IsNullOrWhiteSpace(ServiceName))
                {
                    serviceName = ServiceName;
                }

                if(string.IsNullOrWhiteSpace(serviceName))
                    throw new Exception($"Missing Argument {WinServiceActionInstanceArgs.ServiceName}");

                if(!TryGetTargetStatus(out ServiceTargetStatus status))
                    throw new Exception($"Unexpected Target Status Value ({Status}) Expected valid casting from ({typeof(ServiceTargetStatus)})");

                bool result = false;
                switch (status)
                {
                    case ServiceTargetStatus.Start:
                    {
                        result = WinServiceUtilities.StartService(serviceName, Wait, WaitTimeout);
                        break;
                    }

                    case ServiceTargetStatus.Stop:
                    {
                        result = WinServiceUtilities.StopService(serviceName, Wait, WaitTimeout);
                        break;
                    }

                    case ServiceTargetStatus.Pause:
                    {
                        result = WinServiceUtilities.PauseService(serviceName, Wait, WaitTimeout);
                        break;
                    }

                    case ServiceTargetStatus.Resume:
                    {
                        result = WinServiceUtilities.ResumeService(serviceName, Wait, WaitTimeout);
                        break;
                    }

                    default:
                    {
                        throw new Exception("Unexpected ControlStatus when switching Control cases");
                    }
                }


                return result ? ActionResult.Succeeded() : ActionResult.Failed();
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
                return ActionResult.Failed().WithException(exception);
            }
        }

        private bool TryGetTargetStatus(out ServiceTargetStatus status)
        {
            if (Enum.IsDefined(typeof(ServiceTargetStatus), Status))
            {
                status = (ServiceTargetStatus)Status;

                return true;
            }

            status = ServiceTargetStatus.Stop;
            return false;
        }
    }
}
