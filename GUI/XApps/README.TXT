På mac, fra terminal
xhost + 127.0.0.1

docker build -t x11-apps .
docker run -it --rm --env="DISPLAY=host.docker.internal:0" x11-apps xeyes

Andre applikasjoner
Mac: docker run --rm -ti -e DISPLAY=docker.for.mac.host.internal:0 jamesnetherton/gimp
Windows: docker run --rm -ti -e DISPLAY=host.docker.internal:0 jamesnetherton/gimp

macOS: docker run --rm -ti -e DISPLAY=docker.for.mac.host.internal:0 psharkey/eclipse
Windows: docker run --rm -ti -e DISPLAY=host.docker.internal:0 psharkey/eclipse