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
            // 声明一个队列 
            _channel.QueueDeclare(info.QueueName, info.IsProperties, false, false, null);
            //将队列绑定到交换机
            _channel.QueueBind(info.QueueName, info.ExchangeName, info.RoutingKey, null);
        }
        /// <summary>
        /// 发布消息
        /// </summary>
        public void Publish(T data)
        {
            // 对象转 object[] 发送
            var msg = JsonConvert.SerializeObject(data);
            byte[] bytes = config.Encoding.GetBytes(msg);
            _channel.BasicPublish(info.ExchangeName, info.RoutingKey, null, bytes);
        }

        public void Dispose()
        {
            // 结束
            _channel.Close();
            _connection.Close();
        }
    }
}
