
// See https://aka.ms/new-console-template for more information

using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;

Console.WriteLine("Hello, World!");

var receive = true;
var send = false;

Uri connecturi = new Uri("activemq:tcp://activemq01:61616");
Console.WriteLine("About to connect to " + connecturi);

IConnectionFactory factory = new NMSConnectionFactory(connecturi);

using (IConnection connection = factory.CreateConnection())
using (ISession session = connection.CreateSession())
{


    IDestination destination = SessionUtil.GetDestination(session, "queue://MyFirstQueue");
    Console.WriteLine("Using destination: " + destination);

    // Create a consumer and producer
    using (IMessageConsumer consumer = session.CreateConsumer(destination))
    using (IMessageProducer producer = session.CreateProducer(destination))
    {

        // Start the connection so that messages will be processed.
        connection.Start();
        producer.DeliveryMode = MsgDeliveryMode.Persistent;

        if (send)
        for (int i = 0; i < 20; i++)
        //var i = 1;
        {
            var messData = new PdfInfo
            {
                Fakturanr = $"10000{i}",
                Kundenr = $"{i * 2}000{i}",
                Navn = $"Kåre {i * 3} Knall",
                KID = $"{i * 3}{i * 2}2342342340000555{i * 13}{i * 4}",
                Filtype = "PDF"
            };

            IObjectMessage request = session.CreateObjectMessage(messData);

            // Send a message
            //ITextMessage request = session.CreateTextMessage($"Listen very carefully, I will say this only once. {i}");

            request.NMSCorrelationID = "MyFirstTopic";
            request.Properties["NMSXGroupID"] = messData.Filtype;
            request.Properties["myHeader"] = messData.Kundenr;
            request.NMSTimeToLive = new TimeSpan(0);

            producer.Send(request);

            Console.WriteLine("ObjectmEssage sendt");

        }

        if (send)
        for (int i = 0; i < 5; i++)
        {        
            ITextMessage request = session.CreateTextMessage($"Listen very carefully, I will say this only once. {i}");
            request.NMSTimeToLive = new TimeSpan(0);

            producer.Send(request);
            Console.WriteLine("Textmessage sendt");
        }

        if (receive)
            while (true)
            {
                Console.WriteLine("MEssage received");
                IMessage gmessage = consumer.Receive();
                var mtype = gmessage.GetType();
                Console.WriteLine(mtype.ToString());
                if (gmessage.GetType() == typeof(Apache.NMS.ActiveMQ.Commands.ActiveMQTextMessage))
                {
                    ITextMessage message = (ITextMessage)gmessage; // consumer.Receive() as ITextMessage;
                    if (message == null)
                    {
                        Console.WriteLine("No message received!");
                    }
                    else
                    {
                        Console.WriteLine("Received message with ID:   " + message.NMSMessageId);
                        Console.WriteLine("Received message with text: " + message.Text);
                    }
                }
                else if (gmessage.GetType() == typeof(ActiveMQObjectMessage))
                {
                    ActiveMQObjectMessage message = (ActiveMQObjectMessage)gmessage; // consumer.Receive() as ITextMessage;
                    if (message == null)
                    {
                        Console.WriteLine("No message received!");
                    }
                    else
                    {
                        Console.WriteLine("Received message with ID:   " + message.NMSMessageId);
                        var data = message.Body as PdfInfo;
                        if (data != null)
                        {
                            Console.WriteLine("Received message with navn: " + data.Navn);
                        }
                    }
                }
                else if (gmessage.GetType() == typeof(Apache.NMS.ActiveMQ.Commands.ActiveMQBytesMessage)) {
                    ActiveMQBytesMessage message = (ActiveMQBytesMessage)gmessage; // consumer.Receive() as ITextMessage;
                    if (message == null)
                    {
                        Console.WriteLine("No message received!");
                    }
                    else {
                        var memoryStream = new MemoryStream((message as ActiveMQBytesMessage).Content);
                        //var binaryFormatter = new BinaryFormatter();

                        //memoryStream.Position = 0;

                        PdfInfo data = JsonSerializer.Deserialize<PdfInfo>(memoryStream);
                        Console.WriteLine("Received message with navn: " + data.Navn);
                        //PdfInfo newDog = (Examlple ex = JsonSerializer.Deserialize<Example>(ms);)DeserializeFromStream(stream);
                        
                    }                
                }
            }
    }
}
