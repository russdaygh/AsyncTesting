using System;
using System.Threading;
using System.Web.Mvc;

namespace AsyncTesting
{
    public class MyController : Controller
    {
        public IEmailService EmailService { get; set; }

        public ITaskScheduler TaskScheduler { get; set; }

        public MyController(IEmailService emailService, ITaskScheduler taskScheduler)
        {
            EmailService = emailService;
            TaskScheduler = taskScheduler;
        }

        public ViewResult BeginPasswordReset(string emailAddress)
        {
            TaskScheduler.Run(ctx =>
            {
                ProcessPasswordReset(emailAddress);
            });

            return View();
        }

        private void ProcessPasswordReset(string emailAddress)
        {
            Thread.Sleep(500);
            EmailService.Send(emailAddress);
        }

    }

    public interface IEmailService
    {
        void Send(string emailAddress);
    }

    public interface ITaskScheduler
    {
        void Run(Action<CancellationToken> action);
    }
}