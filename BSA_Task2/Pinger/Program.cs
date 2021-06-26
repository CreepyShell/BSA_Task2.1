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
            service.SendMessageToQueue("pong", "pong");
            service.ListenQueue("ping", (mess) =>
            {
                Console.WriteLine("Message came:"+ mess + "--" + DateTime.Now);
                Thread.Sleep(5000);
                string sendedMessage = "ping" + random.Next(-1000, -1);
                service.SendMessageToQueue(sendedMessage, "pong");
                Console.WriteLine($"Message sended:{sendedMessage}\n");
            });
            Console.ReadLine();
            service.Dispose();
        }
    }
}
