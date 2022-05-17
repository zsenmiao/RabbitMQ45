using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Reflection;

namespace MS.Queue.Framework
{
    public class RabbitMQHandler<T> : IDisposable where T : class
    {

        private IModel _channel;
        private EventingBasicConsumer _consumer;
        private IConnection _connection;
        private MqConfig config;
        private QueueMessageAttribute info;
        public RabbitMQHandler(Action<MqConfig> func)
        {
            info = typeof(T).GetCustomAttribute<QueueMessageAttribute>();
            if (info is null)
                throw new ArgumentException("参数错误");

            config = new MqConfig();
            func(config);

            var factory = new ConnectionFactory
            {
                //设置主机名
                HostName = config.Host,
                //设置心跳时间
                RequestedHeartbeat = config.HeartBeat,
                //设置自动重连
                AutomaticRecoveryEnabled = config.AutomaticRecoveryEnabled,
                //重连时间
                NetworkRecoveryInterval = config.NetworkRecoveryInterval,
                //用户名
                UserName = config.UserName,
                //密码
                Password = config.Password
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(info.QueueName, info.IsProperties, false, false, null);
            _channel.BasicQos(0, 1, false);
        }

        public void Subscribe(Action<T> handler)
        {
            _consumer = new EventingBasicConsumer(_channel);
            //消费者
            _consumer.Received += ((obj, args) =>
            {
                try
                {
                    var t = JsonConvert.DeserializeObject<T>(config.Encoding.GetString(args.Body));
                    handler(t);
                }
                catch (Exception)
                {

                }
                finally
                {
                    _channel?.BasicAck(args.DeliveryTag, false);
                }
            });
            _channel.BasicConsume(info.QueueName, false, _consumer);
        }

        public void Dispose()
        {
            // 结束
            _channel.Close();
            _connection.Close();
        }
    }
}
