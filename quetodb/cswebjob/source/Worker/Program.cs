using System.Text.Json;

Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.Seq("http://seq:5341")
            .CreateLogger();

Console.WriteLine("Hello, Ulriken, starting webworker for ActiveMQ queue!");
Log.Information("User: {Name}, starting webworker!", Environment.UserName);

var receive = true;
//var send = false;

Uri connecturi = new Uri("activemq:tcp://activemq01:61616");
Console.WriteLine("About to connect to " + connecturi);


IConnectionFactory factory = new NMSConnectionFactory(connecturi);

using (IConnection connection = factory.CreateConnection())
using (ISession session = connection.CreateSession())
{


    IDestination destination = SessionUtil.GetDestination(session, "queue://Contact");
    Console.WriteLine("Using destination: " + destination);

    // Create a consumer and producer
    using (IMessageConsumer consumer = session.CreateConsumer(destination))
    using (IMessageProducer producer = session.CreateProducer(destination))
    {

        // Start the connection so that messages will be processed.
        connection.Start();
        producer.DeliveryMode = MsgDeliveryMode.Persistent;

        // if (send)
        // for (int i = 0; i < 20; i++)
        // //var i = 1;
        // {
        //     var messData = new PdfInfo
        //     {
        //         Fakturanr = $"10000{i}",
        //         Kundenr = $"{i * 2}000{i}",
        //         Navn = $"Kåre {i * 3} Knall",
        //         KID = $"{i * 3}{i * 2}2342342340000555{i * 13}{i * 4}",
        //         Filtype = "PDF"
        //     };

        //     IObjectMessage request = session.CreateObjectMessage(messData);

        //     // Send a message
        //     //ITextMessage request = session.CreateTextMessage($"Listen very carefully, I will say this only once. {i}");

        //     request.NMSCorrelationID = "MyFirstTopic";
        //     request.Properties["NMSXGroupID"] = messData.Filtype;
        //     request.Properties["myHeader"] = messData.Kundenr;
        //     request.NMSTimeToLive = new TimeSpan(0);

        //     producer.Send(request);

        //     Console.WriteLine("ObjectmEssage sendt");

        // }

        // if (send)
        // for (int i = 0; i < 5; i++)
        // {        
        //     ITextMessage request = session.CreateTextMessage($"Listen very carefully, I will say this only once. {i}");
        //     request.NMSTimeToLive = new TimeSpan(0);

        //     producer.Send(request);
        //     Console.WriteLine("Textmessage sendt");
        // }

        if (receive)
            while (true)
            {
                Console.WriteLine("Message received");
                IMessage gmessage = consumer.Receive();
                var mtype = gmessage.GetType();
                var messageAsString = mtype.ToString();
                Console.WriteLine(mtype.ToString());
                Log.Information("Webworker received message: {messageAsString}, starting webworker!", messageAsString);
                                
                // if (gmessage.GetType() == typeof(Apache.NMS.ActiveMQ.Commands.ActiveMQTextMessage))
                // {
                //     ITextMessage message = (ITextMessage)gmessage; // consumer.Receive() as ITextMessage;
                //     if (message == null)
                //     {
                //         Console.WriteLine("No message received!");
                //     }
                //     else
                //     {
                //         Console.WriteLine("Received message with ID:   " + message.NMSMessageId);
                //         Console.WriteLine("Received message with text: " + message.Text);
                //     }
                // }
                // else if (gmessage.GetType() == typeof(ActiveMQObjectMessage))
                // {
                //     ActiveMQObjectMessage message = (ActiveMQObjectMessage)gmessage; // consumer.Receive() as ITextMessage;
                //     if (message == null)
                //     {
                //         Console.WriteLine("No message received!");
                //     }
                //     else
                //     {
                //         Console.WriteLine("Received message with ID:   " + message.NMSMessageId);
                //         var data = message.Body as PdfInfo;
                //         if (data != null)
                //         {
                //             Console.WriteLine("Received message with navn: " + data.Navn);
                //         }
                //     }
                // }
                // else 
                
                if (gmessage.GetType() == typeof(Apache.NMS.ActiveMQ.Commands.ActiveMQBytesMessage)) {
                    ActiveMQBytesMessage message = (ActiveMQBytesMessage)gmessage; // consumer.Receive() as ITextMessage;
                    if (message != null)
                    {
                        
                        var memoryStream = new MemoryStream((message as ActiveMQBytesMessage).Content);
                        StreamReader reader = new StreamReader( memoryStream );
                        string jsonString = reader.ReadToEnd();
                        Console.WriteLine("Received message" + jsonString);

                        MessageDto? data = JsonSerializer.Deserialize<MessageDto>(jsonString);
                        Console.WriteLine("Received message with navn: " + data.Username);
                        Log.Information("Webworker received message: {@data}", data);

                        using (IDbConnection db = new MySqlConnection("Server=mysql80;Database=mydb;Uid=user;Pwd=user"))
                        {
                            string insertQuery = @"INSERT INTO mydb.ucmessages (username, useremail, messagetext) VALUES(@username, @useremail, @messagetext)";
                            var result = db.Execute(insertQuery, new
                            {
                                data.Username,
                                data.Useremail,
                                data.Messagetext 
                            });
                            Log.Information("Webworker inserted: {@data} into database", data);
                            Log.CloseAndFlush();
                        }
                                                
                        //var binaryFormatter = new BinaryFormatter();

                        //memoryStream.Position = 0;

                        //var deserializedMessage = binaryFormatter.Deserialize(new RemotingSurrogateSelector(), memoryStream);

                        //MessageDto data = binaryFormatter.Deserialize<MessageDto>(memoryStream);
                        //Console.WriteLine("Received message with navn: " + data.username);
                        //PdfInfo newDog = (Examlple ex = JsonSerializer.Deserialize<Example>(ms);)DeserializeFromStream(stream);
                        
                    }                
                }
            }
    }
}

Log.CloseAndFlush();
