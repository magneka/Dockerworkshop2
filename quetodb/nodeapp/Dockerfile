FROM node:alpine3.15

# Kopierer package json til source katalogen og kjør npm install
# Da får du et layer, dvs cachet alle npm modulene og slipper på
# Kjøre disse hver gang du starter containeren
WORKDIR /source
COPY ./source/package.json .
RUN npm install
