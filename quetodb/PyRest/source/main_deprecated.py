#https://rahmanfadhil.com/flask-rest-api/
#https://github.com/rahmanfadhil/flask-rest-api/blob/master/main.py
#https://stackoverflow.com/questions/27766794/switching-from-sqlite-to-mysql-with-flask-sqlalchemy

from flask import Flask, request
from flask_cors import CORS
from flask_sqlalchemy import SQLAlchemy
from flask_marshmallow import Marshmallow
from marshmallow_sqlalchemy import SQLAlchemySchema, SQLAlchemySchemaOpts
from flask_restful import Api, Resource

import logging
import json
import seqlog


seqlog.log_to_seq(
   server_url="http://seq:5341",   
   level=logging.INFO,
   batch_size=1,
   auto_flush_timeout=1,  # seconds
   override_root_logger=True,
   json_encoder_class = json.encoder.JSONEncoder
)

#logging.basicConfig()
#logging.getLogger('sqlalchemy.engine').setLevel(logging.INFO)
#logging.debug("A log message in level debug")
#logging.info("A log message in level info")
#logging.warning("A log message in level warning")
#logging.error("A log message in level error")
#logging.critical("A log message in level critical")
logging.info("Hello, {name}!", name="Ulriken")


app = Flask(__name__)
CORS(app)
ma = Marshmallow(app)
db = SQLAlchemy(app)
app.config['SQLALCHEMY_TRACK_MODIFICATIONS'] = True
app.config['SQLALCHEMY_DATABASE_URI'] = 'mysql+pymysql://user:user@mysql80/mydb' #'Server=mysql80;Database=mydb;Uid=user;Pwd=user'
api = Api(app)
#app.logger.addHandler(handler)
#logger = logging.getLogger(__name__)

class Ucmessages(db.Model):
    __tablename__ = "ucmessages"
    id = db.Column(db.Integer, primary_key=True)
    Username = db.Column(db.String(255))
    UserEmail = db.Column(db.String(255))
    MessageText = db.Column(db.String(1000))

    def __repr__(self):
        return '<Post %s>' % self.MessageText


class UcmessagesSchema(ma.SQLAlchemyAutoSchema):
    class Meta:   
        model = User
        load_instance = True     
        fields = ("id", "Username", "UserEmail", "MessageText")


message_schema = UcmessagesSchema()
messages_schema = UcmessagesSchema(many=True)


class PostListResource(Resource):
    def get(self):       
        messages = Ucmessages.query.all()
        logging.info("/messages - Returning, {name}!", name=messages_schema.dump(messages))
        return messages_schema.dump(messages)

    def message(self):
        new_message = Post(
            Username=request.json['title'],
            Useremail=request.json['title'],
            Messagetext=request.json['content']
        )
        db.session.add(new_message)
        db.session.commit()
        return post_schema.dump(new_message)


class PostResource(Resource):
    def get(self, message_id):
        message = Post.query.get_or_404(message_id)
        return post_schema.dump(message)

    def patch(self, message_id):
        message = Message.query.get_or_404(message_id)

        if 'Username' in request.json:
            message.Username = request.json['Username']
        if 'Useremail' in request.json:
            message.Useremail = request.json['Useremail']
        if 'Messagetext' in request.json:
            message.Messagetext = request.json['Messagetext']

        db.session.commit()
        return message_schema.dump(message)

    def delete(self, message_id):
        message = Message.query.get_or_404(message_id)
        db.session.delete(message)
        db.session.commit()
        return '', 204


api.add_resource(PostListResource, '/messages')
api.add_resource(PostResource, '/messages_schema/<int:message_id>')


if __name__ == '__main__':    
    #logger.info("Starting python rest api")
    app.run(debug=True)
