using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System;

namespace RabbitMQ.Wrapper
{
    public class RabbitService:IDisposable
    {
        private ConnectionFactory factory;
        private IModel model;
        private IConnection connection;

        private void Inizialize()
        {
            factory = new ConnectionFactory() { HostName = "localhost" };
            connection = factory.CreateConnection();
            model = connection.CreateModel();
        }
        public void ListenQueue(string queue, Action<string> GetMessage)
        {
            Inizialize();
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

        public bool IsEmptyQueue(string queue)
        {
            Inizialize();
            return model.QueueDeclare(queue, autoDelete: false, exclusive: false).MessageCount == 0;
        }

        public void SendMessageToQueue(string message, string key)
        {
            Inizialize();

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
