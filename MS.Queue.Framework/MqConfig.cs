using System;
using System.Text;

namespace MS.Queue.Framework
{
    public class MqConfig
    {
        public string Host { get; set; }
        public ushort HeartBeat { get; set; }
        public bool AutomaticRecoveryEnabled { get; set; }
        public TimeSpan NetworkRecoveryInterval { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Encoding Encoding { get; set; } = Encoding.UTF8;
    }
}
