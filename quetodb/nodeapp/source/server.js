//https://www.digitalocean.com/community/tutorials/use-expressjs-to-get-url-and-post-parameters#prerequisites
const express = require('express');
const app = express();

// Setting up SEQ Logger with Winston API
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

// Setting up an EXPRESS Webserver with folder for static files
app.use(express.json());
app.use(express.urlencoded({ extended: true }));
app.use(express.static('wwwroot'))

//Setting up a STOMP client for ActiveMQ messages
const stomp = require("stomp-client")

// Setting up an endpoint for POST messages
app.post('/api/contact', (req, res) => {
        
    var username = req.body.Name;
    var useremail = req.body.Email;
    var messagetext = req.body.Message;  

    const stompClient = new stomp("activemq01", 61613) // <== merk ikke localhost, men containers indre nett
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
 
    // send user to a fresh message page
    return res.redirect("/index.html");
});

// And finally activating the webserser on a port
app.listen(85,() => {
    logger.debug("Node Express.js server started on port {port}", { port: "85" });  
})