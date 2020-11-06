using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PR.Notifications.Services
{
    public class EmailSender
    {
        public void SendNewUserEmail(string email) //, string subject, string body
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("notifications2k19@gmail.com", "Notification123!"),
                EnableSsl = true,
            };
            smtpClient.Send("notifications2k19@gmail.com", email, "Covid2019", "Jesteś zarażony. Proszę nie wychodzić z domu!");
        }
    }
}
