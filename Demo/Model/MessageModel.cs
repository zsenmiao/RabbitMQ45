using MS.Queue.Framework;
using System;

namespace Model
{
    [QueueMessage("Test.Queue", "Test.Exchange", false)]
    public class MessageModel
    {
        public string Msg { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}
