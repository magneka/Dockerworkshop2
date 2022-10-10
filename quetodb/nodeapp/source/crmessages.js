// https://www.codementor.io/@antonlawrence/building-a-simple-messaging-system-using-activemq-1et7m9nh5a
const stomp = require("stomp-client")

const stompClient = new stomp("activemq01", 61613)

stompClient.connect(() => {
    console.log("producer connected")

    const notification = {      
        Navn: "Kåre knall",
        Fakturanr: "1231234",
        Kundenr: "12",
        KID: "12123123123",
        Filtype: ".csv"
    } 

    stompClient.publish("MyFirstQueue", JSON.stringify(notification))

    stompClient.disconnect()
})
