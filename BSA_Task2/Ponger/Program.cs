using RabbitMQ.Wrapper;
using System;
using System.Threading;

namespace Ponger
{
    class Program
    {
        private static Random random = new Random();
        public static RabbitService service = new RabbitService();
        static void Main(string[] args)
        {
            service.ListenQueue("pong", (mess) =>
            {
                Console.WriteLine("Message came:" + mess + "--" + DateTime.Now);
                Thread.Sleep(2500);
                string sendedMessage = "ping" + random.Next(0, 1000);               
                service.SendMessageToQueue(sendedMessage, "ping");
                Console.WriteLine($"Message sended:{sendedMessage}\n");
            });
            Console.ReadLine();
            service.Dispose();
        }

    }
}
