using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System;

namespace RabbitMQ.Wrapper
{
    public class RabbitService:IDisposable
    {
        private ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };
        private IModel model;
        private IConnection connection;
        public void ListenQueue(string queue, Action<string> GetMessage)
        {
            connection = factory.CreateConnection();
            model = connection.CreateModel();
            model.ExchangeDeclare("ping-pong", ExchangeType.Direct);

            model.QueueDeclare(queue, autoDelete: false, exclusive: false);

            model.QueueBind(queue, "ping-pong", queue);

            var consumeEvent = new EventingBasicConsumer(model);

            consumeEvent.Received += (s, args) =>
            {
                var message = Encoding.UTF8.GetString(args.Body.ToArray());
                GetMessage?.Invoke(message);
                model.BasicAck(args.DeliveryTag, false);
            };
            
            var consumer = model.BasicConsume(queue, true, consumeEvent);
        }

        public void SendMessageToQueue(string message, string key)
        {
            connection = factory.CreateConnection();
            model = connection.CreateModel();

            model.ExchangeDeclare("ping-pong", ExchangeType.Direct);
            

            byte[] body = Encoding.UTF8.GetBytes(message);
            model.BasicPublish("ping-pong", key, null, body);
        }

        public void Dispose()
        {
            model?.Dispose();
            connection?.Dispose();
        }
    }
}
