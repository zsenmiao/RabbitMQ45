using MS.Queue.Framework;
using System;

namespace Model
{
    [QueueMessage("Test2.Ex")]
    public class MessageModel
    {
        public string Msg { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
