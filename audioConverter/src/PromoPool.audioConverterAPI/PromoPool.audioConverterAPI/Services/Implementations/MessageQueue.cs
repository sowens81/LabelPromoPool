using PromoPool.audioConverterAPI.Models;
using PromoPool.audioConverterAPI.Settings;
using RabbitMQ.Client;
using System;
using Newtonsoft.Json;
using System.Text;

namespace PromoPool.audioConverterAPI.Services.Implementations
{
    public class MessageQueue : IMessageQueue
    {

        public ConnectionFactory factory;

        public MessageQueue(IMessageQueueSettings settings)
        {

            var timeSpan = new TimeSpan(0, 0, 0, settings.RequestConnectionTimeout);
            factory = new ConnectionFactory()
            {
                HostName = settings.HostName,
                UserName = settings.UserName,
                Password = settings.Password,
                Port = settings.Port,
                RequestedConnectionTimeout = timeSpan , // milliseconds
            };
    }

        public string AddQueueMessage(AudioSchema audioSchema)
        {

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "AudioConverter",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                var message = JsonConvert.SerializeObject(audioSchema);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "AudioConverter",
                                     basicProperties: null,
                                     body: body);
                return "Sent";
            }

        }
    }
}
