using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading;

namespace AsyncTesting.Tests
{
    [TestClass]
    public class MyControllerTests
    {
        [TestMethod]
        public void BeginPasswordReset_SendsEmail()
        {
            const string emailAddress = "email@domain.com";

            var sendCalled = new ManualResetEvent(false);
            var mockEmailService = new Mock<IEmailService>();
            mockEmailService.Setup(m => m.Send(emailAddress)).Callback(() =>
            {
                sendCalled.Set();
            });

            var controller = new MyController(mockEmailService.Object, new TestTaskScheduler());            

            controller.BeginPasswordReset(emailAddress);

            Assert.IsTrue(sendCalled.WaitOne(TimeSpan.FromSeconds(3)), "Send was never called");
            mockEmailService.Verify(es => es.Send(emailAddress));
        }

        class TestTaskScheduler : ITaskScheduler
        {
            public void Run(Action<CancellationToken> action)
            {
                action.Invoke(new CancellationToken());
            }
        }
    }
}
