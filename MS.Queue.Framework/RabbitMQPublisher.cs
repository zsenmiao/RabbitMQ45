using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Reflection;

namespace MS.Queue.Framework
{
    public class RabbitMQPublisher<T> : IDisposable where T : class
    {
        private IModel _channel;
        private IConnection _connection;
        private MqConfig config;
        private QueueMessageAttribute info;
        private IBasicProperties props;
        public RabbitMQPublisher(Action<MqConfig> func)
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
            // 创建通道
            _channel = _connection.CreateModel();
            // 声明一个Exchange
            _channel.ExchangeDeclare(info.ExchangeName, ExchangeType.Fanout, info.IsProperties, false, null);

            if (!string.IsNullOrWhiteSpace(info.QueueName))
            {
                props = _channel.CreateBasicProperties();
                //props.ContentType = "text/plain";
                props.DeliveryMode = 2;
                props.Expiration = info.Expiration;
                _channel.QueueDeclare(info.QueueName, info.IsProperties, false, false, null);
                _channel.QueueBind(info.QueueName, info.ExchangeName, info.RoutingKey, null);
            }
        }
        /// <summary>
        /// 发布消息
        /// </summary>
        public void Publish(T data)
        {   
            var msg = JsonConvert.SerializeObject(data);
            byte[] bytes = config.Encoding.GetBytes(msg);
            _channel.BasicPublish(info.ExchangeName, info.RoutingKey, props, bytes);
        }

        public void Dispose()
        {
            // 结束
            _channel.Close();
            _connection.Close();
        }
    }
}
