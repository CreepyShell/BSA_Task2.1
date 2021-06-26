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
            service.ListenQueue("ping", (mess) =>
            {
                Console.WriteLine("Message came:" + mess + "--" + DateTime.Now);
                Thread.Sleep(5000);
                string sendedMessage = "pong" + random.Next(0, 1000);               
                service.SendMessageToQueue(sendedMessage, "pong");
                Console.WriteLine($"Message sended:{sendedMessage}\n");
            });
            Console.ReadLine();
            service.Dispose();
        }

    }
}
