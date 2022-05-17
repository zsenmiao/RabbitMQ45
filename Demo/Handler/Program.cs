using Model;
using MS.Queue.Framework;
using Newtonsoft.Json;
using System;

namespace Handler
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var rabbitMqProxy = new RabbitMQHandler<MessageModel>((x) =>
            {
                x.AutomaticRecoveryEnabled = true;
                x.HeartBeat = 30;
                x.NetworkRecoveryInterval = TimeSpan.FromSeconds(60);
                x.Host = "localhost";
                x.UserName = "guest";
                x.Password = "guest";
            });

            rabbitMqProxy.Subscribe(msg =>
            {
                Console.WriteLine(JsonConvert.SerializeObject(msg));
            });

            Console.ReadKey();
        }
    }
}
