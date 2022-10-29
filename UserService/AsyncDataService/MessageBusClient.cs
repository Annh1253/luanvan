using System.Text;
using System.Text.Json;
using UserService.Dtos;
using RabbitMQ.Client;

namespace UserService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        
        private string _userServiceExchangee = "UserServiceExchange";

        public MessageBusClient(IConfiguration configuration)
        {
            this._configuration = configuration;
            var factory = new ConnectionFactory(){HostName = _configuration["RabbitMQHost"], Port = int.Parse(_configuration["RabbitMQPort"])};
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.QueueDeclare(queue: "auth-service-queue",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
                _channel.ExchangeDeclare(exchange: _userServiceExchangee, type: ExchangeType.Direct);
                _channel.QueueBind(queue: "auth-service-queue", exchange: _userServiceExchangee, routingKey: "");
              
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
                Console.WriteLine("---> Connected to Message Bus");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"---> Could not connect to the Message Bus Client: {ex.Message}");
            }
        }
        public void Publish(UserPublishedDto userPublishedDto)
        {
            var message = JsonSerializer.Serialize(userPublishedDto);
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
            _channel.BasicPublish(exchange: _userServiceExchangee, routingKey: "", basicProperties: null, body: body);
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