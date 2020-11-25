using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PR.Notifications.Model;
using PR.Notifications.Services;

namespace PR.Notifications.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        [HttpPost]
        [Authorize]
        public void SendMessage(EmailMessageRequest request)
        {
            EmailSender sender = new EmailSender();
            sender.SendNewUserEmail(request.EmailAddress);
        }

        [HttpPut]
        [AllowAnonymous]
        public IActionResult InvalidAction()
        {
            throw new InvalidOperationException("Symulacja błędu aplikacji Notifications");
        }
    }
}
