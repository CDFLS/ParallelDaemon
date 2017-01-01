using MsgPack;
using System.Reactive;
using System.Reactive.Linq;
using MsgPack.Serialization;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelDaemon
{
    public class RequestDispatcher
    {
        public static void Start(IConnection connection)
        {
            const string queueName = "rpc";
            string exchangeName = GlobalConfig.Config.WebMessageExchange;

            var channel = connection.CreateModel();
            channel.QueueDeclare(queue: queueName,
                                 durable: false,
                                 exclusive: true,
                                 autoDelete: true,
                                 arguments: null);
            channel.BasicQos(0, 1, false);
            channel.ExchangeDeclare(exchangeName, "direct", false, false);

            var consumer = new EventingBasicConsumer(channel);
            var serializer = MessagePackSerializer.Get<Dictionary<MessagePackObject, MessagePackObject>>();

            var observable = Observable.FromEventPattern<BasicDeliverEventArgs>(
                handler => consumer.Received += handler, handler => consumer.Received -= handler);
            observable.Subscribe(x =>
            {
                var message = x.EventArgs.Body;
                var props = x.EventArgs.BasicProperties;
                var replyProps = channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;

                var mpoDict = serializer.UnpackSingleObject(message);

                Console.WriteLine($" [x] Received, reply to {props.ReplyTo}");

                channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                      basicProperties: replyProps, body: serializer.PackSingleObject(
                          new Dictionary<MessagePackObject, MessagePackObject> { { "suck_my_dick", "Go ahead!" } }));
                channel.BasicAck(deliveryTag: x.EventArgs.DeliveryTag,
                  multiple: false);
            });

           /* consumer.Received += (model, ea) =>
            {
                var message = ea.Body;
                var props = ea.BasicProperties;
                var replyProps = channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;

                var mpoDict = serializer.UnpackSingleObject(message);

                Console.WriteLine($" [x] Received, reply to {props.ReplyTo}");

                channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                      basicProperties: replyProps, body: serializer.PackSingleObject(
                          new Dictionary<MessagePackObject, MessagePackObject> { { "suck_my_dick", "Go ahead!" } }));
                channel.BasicAck(deliveryTag: ea.DeliveryTag,
                  multiple: false);
            };
            */

            channel.QueueBind(queueName, exchangeName, "");

            channel.BasicConsume(queue: queueName,
                                 noAck: false,
                                 consumer: consumer);
        }
    }
}
