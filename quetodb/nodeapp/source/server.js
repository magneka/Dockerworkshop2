//https://www.digitalocean.com/community/tutorials/use-expressjs-to-get-url-and-post-parameters#prerequisites
const express = require('express');
const app = express();

// Logger
const winston = require('winston');
const { SeqTransport } = require('@datalust/winston-seq');

const logger = winston.createLogger({
  level: 'debug',
  format: winston.format.combine(  /* This is required to get errors to log with stack traces. See https://github.com/winstonjs/winston/issues/1498 */
    winston.format.errors({ stack: true }),
    winston.format.json(),
  ),
  defaultMeta: { /* application: 'your-app-name' */ },
  transports: [
    new winston.transports.Console({
        format: winston.format.simple(),
    }),
    new SeqTransport({
      serverUrl: "http://seq:5341",
      //apiKey: "Zu69lyksPmMe07eULRcC",
      onError: (e => { console.error(e) }),
      handleExceptions: true,
      handleRejections: true,
    })
  ]
});

// Webserver
app.use(express.json());
app.use(express.urlencoded({ extended: true }));
app.use(express.static('wwwroot'))

//ActiveMQ
const stomp = require("stomp-client")
const stompClient = new stomp("activemq01", 61613) // <== merk ikke localhost, men containers indre nett

app.post('/api/contact', (req, res) => {
    
    console.log(req.body)
    
    var username = req.body.Name;
    var useremail = req.body.Email;
    var messagetext = req.body.Message;  

    stompClient.connect(() => {
        console.log("ActiveMQ is connected")
    
        const notification = {      
            Username: username,
            Useremail: useremail,
            Messagetext: messagetext,
        }
    
        stompClient.publish("Contact", JSON.stringify(notification))    
      
        logger.debug("/api/contact, Message sendt to queue: {Username}/{Useremail} messagetext {Messagetext}", notification)

        stompClient.disconnect()
    
    })
 
    //console.log(`Contacted by ${username}, ${useremail} regarding ${messagetext}`);
    return res.redirect("/index.html");
});

app.listen(85,() => {
    console.log("Started on PORT 85");
    logger.debug("Server started on port {port}", { port: "85" });
  
})