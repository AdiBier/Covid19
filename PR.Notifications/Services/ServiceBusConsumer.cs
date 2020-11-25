using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PR.Notifications.Services;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PR.Notifications.Services
{
    public class ServiceBusConsumer
    {
        private readonly QueueClient _queueClient;
        private const string QueueName = "messages";

        public ServiceBusConsumer(IConfiguration configuration)
        {
            QueueClient queueClient = new QueueClient(configuration.GetConnectionString("ServiceBusConnectionString"), QueueName);
            QueueClient queueClient1 = queueClient;
            _queueClient = queueClient1;
        }
        public void Register()
        {
            var options = new MessageHandlerOptions((e) => Task.CompletedTask)
            {
                AutoComplete = false
            };
            _queueClient.RegisterMessageHandler(ProcessMessage, options);
        }
        private async Task ProcessMessage(Message message, CancellationToken token)
        {
            var payload = JsonConvert.DeserializeObject<MessagePayLoad>(Encoding.UTF8.GetString(message.Body));

            if (payload.EventName == "NewUserRegistered")
            {
                EmailSender sender = new EmailSender();
                sender.SendNewUserEmail(payload.UserEmail);
            }
            await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }
    }
    public class MessagePayLoad
    {
        public string EventName { get; set; }
        public string UserEmail { get; set; }
    }
}
