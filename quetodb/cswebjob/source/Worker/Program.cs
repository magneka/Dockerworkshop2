
Uri connecturi = new Uri("activemq:tcp://activemq01:61616");

IConnectionFactory activeMQfactory = new NMSConnectionFactory(connecturi);

using (IDbConnection db = new MySqlConnection("Server=mysql80;Database=mydb;Uid=user;Pwd=user"))
using (IConnection activeMQConnection = activeMQfactory.CreateConnection())
using (ISession activeMQSession = activeMQConnection.CreateSession())
{

    Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.Seq("http://seq:5341")
            .CreateLogger();

    Console.WriteLine("Hello, Ulriken, starting webworker for ActiveMQ queue!");
    Log.Information("User: {Name}, starting webworker!", Environment.UserName);

    IDestination destination = SessionUtil.GetDestination(activeMQSession, "queue://Contact");
    Console.WriteLine("Using destination: " + destination);

    // Create a consumer 
    using (IMessageConsumer consumer = activeMQSession.CreateConsumer(destination))
    {

        // Start the connection so that messages will be processed.
        activeMQConnection.Start();  

        while (true)
        {
            try
            {
                IMessage gmessage = consumer.Receive();
                var mtype = gmessage.GetType();
                //var messageAsString = mtype.ToString();
                
                Log.Information("Webworker received message of type: " + mtype.ToString());                                            
                
                if (gmessage.GetType() == typeof(Apache.NMS.ActiveMQ.Commands.ActiveMQBytesMessage)) {
                    ActiveMQBytesMessage message = (ActiveMQBytesMessage)gmessage; // consumer.Receive() as ITextMessage;
                    if (message != null)
                    {
                        
                        var memoryStream = new MemoryStream((message as ActiveMQBytesMessage).Content);
                        StreamReader reader = new StreamReader( memoryStream );
                        string jsonString = reader.ReadToEnd();
                        Console.WriteLine("Received ActiveMQBytesMessage message" + jsonString);

                        MessageDto? data = JsonSerializer.Deserialize<MessageDto>(jsonString);
                        Console.WriteLine("Received message with navn: " + data?.Username);
                        Log.Information("Webworker received message: {@data}", data);
                    
                        string insertQuery = @"INSERT INTO mydb.ucmessages (username, useremail, messagetext) VALUES(@username, @useremail, @messagetext)";
                        var result = db.Execute(insertQuery, new
                        {
                            data?.Username,
                            data?.Useremail,
                            data?.Messagetext 
                        });
                                                                                        
                        Log.Information("Webworker inserted: {@data} into database", data);                        
                    }                
                }
                else {
                    Log.Information("Unknown message type received");
                }
                //
                
            }
            catch (System.Exception ex)
            {
               Log.Error("Webworker exception:" + ex.Message);
            }
            
        }
    }
    Log.CloseAndFlush();
}


