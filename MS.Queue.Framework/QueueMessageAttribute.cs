using System;

namespace MS.Queue.Framework
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class QueueMessageAttribute : Attribute
    {

        public QueueMessageAttribute(string queueName, string exchangeName, bool isProperties = false)
        {
            ExchangeName = exchangeName;
            QueueName = queueName;
            RoutingKey = queueName;
            IsProperties = isProperties;
        }
        /// <summary>
        /// 交换机名称
        /// </summary>
        public string ExchangeName { get; set; }

        /// <summary>
        /// 队列名称
        /// </summary>
        public string QueueName { get; private set; }

        public string RoutingKey { get; set; }

        public bool IsProperties { get; set; }
    }
}
