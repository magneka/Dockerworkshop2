from flask import Flask, jsonify, request
from flask_cors import CORS
from flaskext.mysql import MySQL

#if settings.DEBUG: 
#import debugpy
#debugpy.listen(('0.0.0.0', 5678))
#print('Attached!')

app = Flask(__name__)

#Set database credentials in config.
app.config['MYSQL_DATABASE_USER'] = 'user'
app.config['MYSQL_DATABASE_PASSWORD'] = 'user'
app.config['MYSQL_DATABASE_DB'] = 'mydb'
app.config['MYSQL_DATABASE_HOST'] = 'mysql80'
mysql = MySQL()
mysql.init_app(app)


@app.route('/messages', methods=['GET'])
def helloIndex():
    try:
        print('eh')
        conn = mysql.connect()
        cursor = conn.cursor()
        sqlstmt = "select * from ducmessages"
        #cursor.execute("""select * from ducmessages""")
        cursom.execute(sqlstmt)
        rows = cursor.fetchall()
        return jsonify(rows)
    except Exception as e:
        print(e)
    finally:
        cursor.close()
        conn.close()

@app.route('/message/<id>', methods=['GET'])
def add_message(id):
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

@app.route('/message', methods=['POST'])  
def post_message():
        try:
            conn = mysql.connect()
            cursor = conn.cursor()
            _username = request.args['username']
            _useremail = request.args['userEmail']
            _messagetext = request.args['messageText']
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

@app.route('/message/<id>', methods=['PUT']) 
def put_message(id):
        try:
            conn = mysql.connect()
            cursor = conn.cursor()
            _username = request.args['username']
            _useremail = request.args['userEmail']
            _messagetext = request.args['messageText']
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

@app.route('/message/<id>', methods=['DELETE']) 
def delete_message(id):
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


app.run(host='0.0.0.0', port=5000)