#https://www.opentechguides.com/how-to/article/python/210/flask-mysql-crud.html

from flask import Flask, jsonify, request
from flask_cors import CORS
from flaskext.mysql import MySQL
from flask_restful import Resource, Api

#if settings.DEBUG: 
#import debugpy
#debugpy.listen(('0.0.0.0', 5678))
#print('Attached!')

#import logging
#import json
#import seqlog

#seqlog.log_to_seq(
#   server_url="http://seq:5341",   
#   level=logging.INFO,
#   batch_size=1,
#   auto_flush_timeout=1,  # seconds
#   override_root_logger=True,
#   json_encoder_class = json.encoder.JSONEncoder
#)
#logging.info("Hello, {name}!", name="Ulriken")

#Create an instance of Flask
app = Flask(__name__)

#Create an instance of MySQL
mysql = MySQL()

#Create an instance of Flask RESTful API
api = Api(app)

#Set database credentials in config.
app.config['MYSQL_DATABASE_USER'] = 'user'
app.config['MYSQL_DATABASE_PASSWORD'] = 'user'
app.config['MYSQL_DATABASE_DB'] = 'mydb'
app.config['MYSQL_DATABASE_HOST'] = 'mysql80'

#Initialize the MySQL extension
mysql.init_app(app)

#Get All Users, or Create a new user
class UcMessages(Resource):
    def get(self):
        try:
            conn = mysql.connect()
            cursor = conn.cursor()
            cursor.execute("""select * from ucmessages""")
            rows = cursor.fetchall()
            return jsonify(rows)
        except Exception as e:
            print(e)
        finally:
            cursor.close()
            conn.close()

    def post(self):
        try:
            conn = mysql.connect()
            cursor = conn.cursor()
            _username = request.form['username']
            _useremail = request.form['userEmail']
            _messagetext = request.form['messageText']
            insert_user_cmd = """INSERT INTO ucmessages(username, userEmail, messagetext) 
                                VALUES(%s, %s, %s)"""
            cursor.execute(insert_user_cmd, (_username, _useremail, _messagetext))
            conn.commit()
            response = jsonify(message='User added successfully.', id=cursor.lastrowid)
            response.data = cursor.lastrowid
            response.status_code = 200
        except Exception as e:
            print(e)
            response = jsonify('Failed to add user.')         
            response.status_code = 400 
        finally:
            cursor.close()
            conn.close()
            return(response)

class UcMessage(Resource):
    def get(self, id):
        try:
            conn = mysql.connect()
            cursor = conn.cursor()
            cursor.execute('select * from ucmessages where id = %s',id)
            rows = cursor.fetchall()
            return jsonify(rows)
        except Exception as e:
            print(e)
        finally:
            cursor.close()
            conn.close()

    def put(self, id):
        try:
            conn = mysql.connect()
            cursor = conn.cursor()
            _username = request.form['username']
            _useremail = request.form['userEmail']
            _messagetext = request.form['messageText']
            update_user_cmd = """update ucmessages 
                                    set username=%s, useremail=%s, messagetext=%s
                                    where id=%s"""
            cursor.execute(update_user_cmd, (_username, _useremail, _messagetext, id))
            conn.commit()
            response = jsonify('message updated successfully.')
            response.status_code = 200
        except Exception as e:
            print(e)
            response = jsonify('Failed to update message.')         
            response.status_code = 400
        finally:
            cursor.close()
            conn.close()    
            return(response)   

    def delete(self, id):
        try:
            conn = mysql.connect()
            cursor = conn.cursor()
            cursor.execute('delete from ucmessages where id = %s',id)
            conn.commit()
            response = jsonify('Message deleted successfully.')
            response.status_code = 200
        except Exception as e:
            print(e)
            response = jsonify('Failed to delete message.')         
            response.status_code = 400
        finally:
            cursor.close()
            conn.close()    
            return(response)      
      

#API resource routes
api.add_resource(UcMessages, '/messages', endpoint='messages')
api.add_resource(UcMessage, '/message/<int:id>', endpoint='message')

if __name__ == "__main__":
    #app.run(debug=True)
    app.run(host="0.0.0.0", port="80", debug=True)

