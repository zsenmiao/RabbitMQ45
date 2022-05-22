using System;

namespace MS.Queue.Framework
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class QueueMessageAttribute : Attribute
    {

        public QueueMessageAttribute(string exchangeName, string queueName = "", bool isProperties = true, string expiration = "10000")
        {
            Expiration = expiration;
            ExchangeName = exchangeName;
            QueueName = queueName;
            RoutingKey = queueName;
            IsProperties = isProperties;
        }
        public string Expiration { get; set; }
        /// <summary>
        /// 交换机名称
        /// </summary>
        public string ExchangeName { get; set; }

        /// <summary>
        /// 队列名称
        /// </summary>
        public string QueueName { get; set; }

        public string RoutingKey { get; set; }

        public bool IsProperties { get; set; }
    }
}
