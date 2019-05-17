FROM ubuntu:16.04
WORKDIR /App

RUN apt-get update
RUN apt-get install -y   default-jre   default-jdk   git curl
RUN apt-get update
RUN curl -sL  https://deb.nodesource.com/setup_8.x | bash -
RUN apt-get install -y nodejs
RUN npm -v && node -v

COPY . .
RUN npm install ; exit 0

ENTRYPOINT [ "npm", "run" ]