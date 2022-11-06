Ressurser:
https://huongdanjava.com/install-apache-activemq-using-docker-compose.html
https://hub.docker.com/r/webcenter/activemq/

logg på konsollet med 
http://localhost:8161/admin/
user/pw: admin/admin

Post på køen (Write)
curl -XPOST -u admin:admin -d "body={id: 1, name: 'ola dunk'}" http://localhost:8161/api/message/orders.input?type=queue

Du kan gå inn i køen og se meldinene dine
http://localhost:8161/admin/browse.jsp?JMSDestination=orders.input


{
  "pbiScheduleId": 17,  
  "scheduleId": 3,  
  "reportId": 24,  
  "reportName": null,  
  "scheduleName": "test 6 oppdatert",  
  "pbiGgroupId": null, 
  "pbiReportId": null,  
  "pbiDatasetId": null,  
  "clientId": null,  
  "pbiUsername": null,  
  "pbiPassword": null,  
  "roles": null,  
  "displayInMenu": null,  
  "usernameRequired": null,  
  "filter": null,  
  "settings": null,  
  "defaultReport": null,  
  "defaultScheduleIntervalId": null,  
  "defaultDataIntervalId": null,
  "sqlCheckEmptyReport": null,  
  "reportGroup": null,  
  "concentUserName": "mary.kaland2@kred.no",  
  "departmentId": 20,  
  "lastSchedule": null,  
  "nextSchedule": null,  
  "schedIntvalId": 2,  
  "schedIntvalName": null,  
  "dataIntvalId": null,  
  "dataIntvalName": null,  
  "sendNotice": true,  
  "isEmpty": true,  
  "allUserCreditors": false,  
  "calculatedFromDate": null,  
  "calculatedToDate": null,  
  "listActor": [    
    { "actorId": 0,      "customerNo": "600220",      "departmentId": 20,      "actorName": null"    },    
    { "actorId": 0,      "customerNo": "600328",      "departmentId": 20,      "actorName": null"    },    
    { "actorId": 0,      "customerNo": "500858",      "departmentId": 20,      "actorName": null"    }  
  ],  
  "scheduledReport": null,
  "rowId": "17-1"
}