consol/centos-icewm-vnc

Start fra compose
Start en remote desktop vnc klient
pek på localhost:5901
Passordet er 1234

Skulle virket også i nettleer 
på http://localhost:6901/?password=1234

Fungerer denne fra docker cli? Hva er forskjellen på denne og compose varianten
docker run -it -e VNC_PW=1234 -p 5901:5901 -p 6901:6901 --name centosvnc consol/centos-xfce-vnc bash