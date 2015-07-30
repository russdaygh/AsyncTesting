using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading;

namespace AsyncTesting
{
    public class MyController : Controller
    {
        public IEmailService EmailService { get; set; }

        public MyController(IEmailService emailService)
        {
            EmailService = emailService;
        }

        public ViewResult BeginPasswordReset(string emailAddress)
        {
            BeginPasswordResetAsync(emailAddress);

            return View();
        }

        private Task BeginPasswordResetAsync(string emailAddress)
        {
            return Task.Run(delegate
            {
                Thread.Sleep(500);
                EmailService.Send(emailAddress);
            });
        }

    }

    public interface IEmailService
    {
        void Send(string emailAddress);
    }
}