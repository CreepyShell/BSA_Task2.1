using RabbitMQ.Wrapper;
using System;
using System.Threading;

namespace Pinger
{
    class Program
    {
        private static Random random = new Random();
        private static RabbitService service = new RabbitService();
        static void Main(string[] args)
        {
            if (service.IsEmptyQueue("pong") && service.IsEmptyQueue("ping"))//в очереди не должно быть больше одного сообщения так же как и
                service.SendMessageToQueue("startedPong", "pong");//в пинг-понге больше одного мячика


            service.ListenQueue("ping", (mess) =>
            {
                Console.WriteLine("Message came:" + mess + "--" + DateTime.Now);
                Thread.Sleep(2500);
                string sendedMessage = "ping" + random.Next(-1000, -1);
                service.SendMessageToQueue(sendedMessage, "pong");
                Console.WriteLine($"Message sended:{sendedMessage}\n");
            });
            Console.ReadLine();
            service.Dispose();
        }
    }
}
