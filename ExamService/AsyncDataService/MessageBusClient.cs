using System.Text;
using System.Text.Json;
using ExamService.AsyncDataServices;
using ExamService.Dtos;
using RabbitMQ.Client;

namespace ExamService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        
        private string _examServiceExchangee = "ExamServiceExchange";
        private string _examChallengeQueue = "exam-challenge-service-queue";

        public MessageBusClient(IConfiguration configuration)
        {
            this._configuration = configuration;
            var factory = new ConnectionFactory(){HostName = _configuration["RabbitMQHost"], Port = int.Parse(_configuration["RabbitMQPort"])};
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.QueueDeclare(queue: _examChallengeQueue,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
                _channel.ExchangeDeclare(exchange: _examServiceExchangee, type: ExchangeType.Direct);
                _channel.QueueBind(queue: _examChallengeQueue, exchange: _examServiceExchangee, routingKey: "");
              
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
                Console.WriteLine("---> Connected to Message Bus");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"---> Could not connect to the Message Bus Client: {ex.Message}");
            }
        }
        public void PublishExam(ExamPublishedDto examPublishedDto)
        {
            var message = JsonSerializer.Serialize(examPublishedDto);
            if(_connection.IsOpen)
            {

                Console.WriteLine("---> RabbitMQ connection is OPEN, sending Message...");
                SendMessage(message);
                //To do send message
            }
            else
            {
                Console.WriteLine("---> RabbitMQ connection is CLOSE, not sending");

            }
        }

        public void PublishQuestion(QuestionPublishedDto questionPublishedDto)
        {
            var message = JsonSerializer.Serialize(questionPublishedDto);
            if(_connection.IsOpen)
            {

                Console.WriteLine("---> RabbitMQ connection is OPEN, sending Message...");
                SendMessage(message);
                //To do send message
            }
            else
            {
                Console.WriteLine("---> RabbitMQ connection is CLOSE, not sending");

            }
        }

         public void PublishOption(OptionPublishedDto optionPublishedDto)
        {
            var message = JsonSerializer.Serialize(optionPublishedDto);
            if(_connection.IsOpen)
            {

                Console.WriteLine("---> RabbitMQ connection is OPEN, sending Message...");
                SendMessage(message);
                //To do send message
            }
            else
            {
                Console.WriteLine("---> RabbitMQ connection is CLOSE, not sending");

            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: _examServiceExchangee, routingKey: "", basicProperties: null, body: body);
            Console.WriteLine($"---> We have sent message: {message}");
        }

      

        public void Dispose()
        {
            Console.WriteLine("---> Message Bus Disposed");
            if(_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("---> RabbitMQ Connection Shutdown");
        }
    }
}