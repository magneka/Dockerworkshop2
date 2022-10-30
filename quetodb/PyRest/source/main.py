#https://rahmanfadhil.com/flask-rest-api/
#https://github.com/rahmanfadhil/flask-rest-api/blob/master/main.py
#https://stackoverflow.com/questions/27766794/switching-from-sqlite-to-mysql-with-flask-sqlalchemy

from flask import Flask, request
from flask_sqlalchemy import SQLAlchemy
from flask_marshmallow import Marshmallow
from flask_restful import Api, Resource

app = Flask(__name__)
app.config['SQLALCHEMY_DATABASE_URI'] = 'mysql://user:user@mysql80/mydb' #'Server=mysql80;Database=mydb;Uid=user;Pwd=user'
db = SQLAlchemy(app)
ma = Marshmallow(app)
api = Api(app)


class Message(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    Username = db.Column(db.String(255))
    UserEmail = db.Column(db.String(255))
    MessageText = db.Column(db.String(1000))

    def __repr__(self):
        return '<Post %s>' % self.MessageText


class MessageSchema(ma.Schema):
    class Meta:
        fields = ("id", "Username", "UserEmail", "MessageText")


message_schema = MessageSchema()
messages_schema = MessageSchema(many=True)


class PostListResource(Resource):
    def get(self):
        messages = Message.query.all()
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
    app.run(debug=True)