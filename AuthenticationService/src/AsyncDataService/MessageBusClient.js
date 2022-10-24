const amqp = require("amqplib/callback_api");
const userServiceQueue = "user-service-queue";
const authServiceExchange = "AuthServiceExchange";

class MessagePublisher {
  publishMessage(message) {
    amqp.connect(
      `amqp://${process.env.RABBITMQ_HOST_K8S}:${process.env.RABBITMQ_PORT_K8S}`,
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

            channel.bindQueue(userServiceQueue, authServiceExchange, "");

            channel.publish(
              authServiceExchange,
              "",
              Buffer.from(JSON.stringify(message))
            );
            console.log(" [x] Sent %s", message);
          } catch (ex) {
            throw ex;
          }
        });
      }
    );
  }
}

module.exports = new MessagePublisher();