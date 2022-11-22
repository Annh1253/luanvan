var amqp = require("amqplib/callback_api");
var EventProcessor = require("../EventProcessing/EventProcessor");

amqp.connect(
  // `amqp://${process.env.RABBITMQ_HOST}:${process.env.RABBITMQ_PORT}`,
  `amqp://${process.env.RABBITMQ_HOST_K8S}:${process.env.RABBITMQ_PORT_K8S}`,
  function (error0, connection) {
    if (error0) {
      throw error0;
    }
    connection.createChannel(function (error1, channel) {
      if (error1) {
        throw error1;
      }
      var exchange = "UserServiceExchange";
      var queue = "auth-service-queue";
      channel.assertExchange(exchange, "direct", {
        durable: false,
      });

      channel.assertQueue(
        queue,
        {
          durable: true,
        },
        function (error2, q) {
          if (error2) {
            throw error2;
          }
          console.log(" [*] Waiting for logs. To exit press CTRL+C");
          channel.bindQueue(queue, exchange, "");

          channel.consume(
            queue,
            function (msg) {
              console.log(
                " [x] %s: '%s'",
                msg.fields.routingKey,
                msg.content.toString()
              );
              message = JSON.parse(msg.content.toString());
              console.log(message);
              EventProcessor.processEvent(message);
            },
            {
              noAck: true,
            }
          );
        }
      );
    });
  }
);

module.exports = amqp;
