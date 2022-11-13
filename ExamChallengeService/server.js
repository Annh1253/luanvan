require('dotenv').config()
const express = require("express");
const app = express();
const server = require("http").createServer(app);
const socketIo = require("./socket.io");
const mongoose = require("mongoose");
const db = require("./config/db");
const route = require("./routes");
const port = 3005;
const cors = require("cors");
const EventType = require("./EventProcessing/EventType")
const messageBusSubscriber = require("./AsyncDataService/MessageBusSubscriber.js");

console.log("Event: " + EventType.NewExamCreate);
app.use(
  cors({
    origin: "*",
  })
);

app.use(
  express.urlencoded({
    limit: "50mb",
    extended: true,
  })
);
app.use(express.json({ limit: "50mb" }));

db.connect();
socketIo.connect(server)


server.listen(port || 6005, () => {
  console.log(
    "Backend server is running at port: " +
      port +
      "\n RabbitMQ server is running at : " +
      process.env.RABBITMQ_HOST_K8S +
      " : " +
      process.env.RABBITMQ_PORT_K8S
  );
});
