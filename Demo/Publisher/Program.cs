using Model;
using MS.Queue.Framework;
using System;
using System.Threading;

namespace Publisher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var rabbitMqProxy = new RabbitMQPublisher<MessageModel>((x) =>
            {
                x.AutomaticRecoveryEnabled = true;
                x.HeartBeat = 30;
                x.NetworkRecoveryInterval = TimeSpan.FromSeconds(60);
                x.Host = "localhost";
                x.UserName = "guest";
                x.Password = "guest";
            });

            while (true)
            {
                var log = new MessageModel
                {
                    CreateDateTime = DateTime.Now,
                    Msg = "yes"
                };
                rabbitMqProxy.Publish(log);
                Thread.Sleep(2000);
            }
        }
    }
}
