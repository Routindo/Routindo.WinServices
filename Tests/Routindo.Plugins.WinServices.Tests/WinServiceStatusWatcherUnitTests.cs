using Microsoft.VisualStudio.TestTools.UnitTesting;
using Routindo.Plugins.WinServices.Components.ServiceWatcher;

namespace Routindo.Plugins.WinServices.Tests
{
    [TestClass]
    public class WinServiceStatusWatcherUnitTests
    {
        [TestMethod]
        public void WinServiceFailsOnNoNameSpcified()
        {
            WinServiceWatcher winServiceStatusWatcher = new WinServiceWatcher()
            {
                Status = 1
            };
            var watcherResult =  winServiceStatusWatcher.Watch();
            Assert.IsNotNull(watcherResult);
            Assert.IsFalse(watcherResult.Result);
            Assert.IsNotNull(watcherResult.AttachedException);
        }


        [TestMethod]
        public void WinServiceFailsOnNotExistingService()
        {
            WinServiceWatcher winServiceStatusWatcher = new WinServiceWatcher()
            {
                ServiceName = "YOUNESCHEIKH",
                Status = 1
            };
            var watcherResult = winServiceStatusWatcher.Watch();
            Assert.IsNotNull(watcherResult);
            Assert.IsFalse(watcherResult.Result);
            Assert.IsNotNull(watcherResult.AttachedException);
        }


    }
}
