
# RabbitMQ帮助类




## 安装


```bash
Install-Package MSQueueFramework -Version 1.0.0
```
    
## 定义模型

```c#
    [QueueMessage("Test.Exchange")]
    public class MessageModel
    {
        public string Msg { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
```

## 发送端

```c#
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
```

## 接收端

```c#
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
```