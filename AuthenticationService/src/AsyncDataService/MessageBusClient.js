const amqp = require("amqplib/callback_api");
const userServiceQueue = "user-service-queue";
const examServiceQueue = "exam-service-queue";

const authServiceExchange = "AuthServiceExchange";

class MessagePublisher {
  publishMessage(message) {
    amqp.connect(
      `amqp://${process.env.RABBITMQ_HOST_K8S}:${process.env.RABBITMQ_PORT_K8S}`,
      // `amqp://${process.env.RABBITMQ_HOST}:${process.env.RABBITMQ_PORT}`,
      function (error0, connection) {
        if (error0) {
          throw error0;
        }
        console.log("Connected to Message bus");
        connection.createChannel(function (error1, channel) {
          if (error1) {
            throw error1;
          }

          try {
            channel.assertExchange(authServiceExchange, "direct", {
              durable: false,
            });

            channel.assertQueue(userServiceQueue, {
              durable: true,
            });

            channel.assertQueue(examServiceQueue, {
              durable: true,
            });

            channel.bindQueue(userServiceQueue, authServiceExchange, "");
            channel.bindQueue(examServiceQueue, authServiceExchange, "");

            channel.publish(
              authServiceExchange,
              "",
              Buffer.from(JSON.stringify(message))
            );
            console.log(" [x] Sent %s", message, "To exchange : " + authServiceExchange + "which binded to : " + userServiceQueue);
          } catch (ex) {
            throw ex;
          }
        });
      }
    );
  }
}

module.exports = new MessagePublisher();
