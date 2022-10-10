// See https://aka.ms/new-console-template for more information
[Serializable]
class PdfInfo 
{
    public String? Navn { get; set; }
    public String? Fakturanr { get; set; }
    public String? Kundenr { get; set; }
    public String? KID { get; set; }
    public String? Filtype { get; set; }
}

public class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

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

                for (int i = 0; i < 200; i++)
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

                    //IObjectMessage request = session.CreateObjectMessage(messData);

                    // Send a message
                    ITextMessage request = session.CreateTextMessage($"Listen very carefully, I will say this only once. {i}");

                    //request.NMSCorrelationID = "MyFirstTopic";
                    //request.Properties["NMSXGroupID"] = messData.Filtype;
                    //request.Properties["myHeader"] = messData.Kundenr;
                    request.NMSTimeToLive = new TimeSpan(0);

                    producer.Send(request);

                    Console.WriteLine("MEssage sendt");

                    
                }


                var receive = true;
                if (receive)
                while (true)
                {
                    Console.WriteLine("MEssage received");
                    IMessage gmessage = consumer.Receive();
                    if (gmessage.GetType() == typeof(ITextMessage))
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
                }
            }
        }
    }
}