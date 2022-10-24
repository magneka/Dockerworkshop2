CREATE TABLE student (
    id int,
    name VARCHAR(255)
);

INSERT INTO student(id, name) VALUES
(1,'A'),
(2,'B'),
(3,'C');

CREATE TABLE ucmessages(
    id int NOT NULL AUTO_INCREMENT,
    username varchar (255),
    useremail VARCHAR(255),
    messagetext varchar (10000),
    PRIMARY KEY (id)
)
