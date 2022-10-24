//https://www.digitalocean.com/community/tutorials/use-expressjs-to-get-url-and-post-parameters#prerequisites
const express = require('express');
const app = express();

app.use(express.json());
app.use(express.urlencoded({ extended: true }));
app.use(express.static('wwwroot'))

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
    
    })
 

    //stompClient.disconnect()

    console.log(`Contacted by ${username}, ${useremail} regarding ${messagetext}`);
    return res.redirect("/index.html");
});

app.listen(85,() => {
console.log("Started on PORT 85");
})