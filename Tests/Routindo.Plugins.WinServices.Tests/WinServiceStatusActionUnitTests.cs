using Microsoft.VisualStudio.TestTools.UnitTesting;
using Routindo.Contract;
using Routindo.Contract.Arguments;
using Routindo.Plugins.WinServices.Components.ControlServiceAction;

namespace Routindo.Plugins.WinServices.Tests
{
    [TestClass]
    public class WinServiceStatusActionUnitTests
    {
        [TestMethod]
        public void ActionFailsOnServiceSpecificName()
        {
            WinServiceAction controlServiceAction = new WinServiceAction()
            {
                Status = 1,
                Wait = false,
                WaitTimeout = 0
            };

            var actionResult = controlServiceAction.Execute(ArgumentCollection.New());
            Assert.IsNotNull(actionResult);
            Assert.IsFalse(actionResult.Result);
            Assert.IsNotNull(actionResult.AttachedException);
        }

        [TestMethod]
        public void ActionFailsOnServiceNotFound()
        {
            WinServiceAction controlServiceAction = new WinServiceAction()
            {
                ServiceName = "YOUNESCHEIKH",
                Status = 1,
                Wait = false,
                WaitTimeout = 0
            };

            var actionResult = controlServiceAction.Execute(ArgumentCollection.New());
            Assert.IsNotNull(actionResult);
            Assert.IsFalse(actionResult.Result);
            Assert.IsNotNull(actionResult.AttachedException);
        }


        [TestMethod]
        public void ActionFailsOnWrongStatusToAct()
        {
            WinServiceAction controlServiceAction = new WinServiceAction()
            {
                ServiceName = "YOUNESCHEIKH",
                Status = 28,
                Wait = false,
                WaitTimeout = 0
            };

            var actionResult = controlServiceAction.Execute(ArgumentCollection.New());
            Assert.IsNotNull(actionResult);
            Assert.IsFalse(actionResult.Result);
            Assert.IsNotNull(actionResult.AttachedException);
        }
    }
}
