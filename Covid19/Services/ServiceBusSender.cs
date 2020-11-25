using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Covid19.Services
{
    public class ServiceBusSender
    {
        private readonly QueueClient _queueClient;
        private const string QueueName = "messages";

        public ServiceBusSender(IConfiguration configuration)
        {
            QueueClient queueClient = new QueueClient(configuration.GetConnectionString("ServiceBusConnectionString"), QueueName);
            QueueClient queueClient1 = queueClient;
            _queueClient = queueClient1;
        }
        public async Task SendMessage(MessagePayLoad payload)
        {
            string data = JsonConvert.SerializeObject(payload);
            Message message = new Message(Encoding.UTF8.GetBytes(data));
            await _queueClient.SendAsync(message);
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
