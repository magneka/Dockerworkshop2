FROM python:3.9

RUN pip3 install --upgrade --force-reinstall flask && \
     pip3 install pytest 

WORKDIR /source
COPY ./source/requirements.txt .
RUN pip3 install -r requirements.txt

