const amqp = require("amqplib/callback_api");
const examServiceQueue = "exam-service-queue";
const examChallengeServiceExchange = "ExamChallengeServiceExchange";

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
            channel.assertExchange(examChallengeServiceExchange, "direct", {
              durable: false,
            });

            channel.assertQueue(examServiceQueue, {
              durable: true,
            });

            channel.bindQueue(examServiceQueue, examChallengeServiceExchange, "");

            channel.publish(
              examChallengeServiceExchange,
              "",
              Buffer.from(JSON.stringify(message))
            );
            console.log(" [x] Sent %s", message, "To exchange : " + examChallengeServiceExchange + "which binded to : " + examServiceQueue);
          } catch (ex) {
            throw ex;
          }
        });
      }
    );
  }
}

module.exports = new MessagePublisher();
