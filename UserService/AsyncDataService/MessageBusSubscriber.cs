using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using UserService.EventProcessing;

namespace UserService.AsyncDataService
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IEventProcessor _eventProcessor;
        private IConnection _connection;
        private IModel _channel;
        private string _queueName = "user-service-queue";

        private string _authServiceExchangeName = "AuthServiceExchange";

        public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor)
        {
            this._configuration = configuration;
            this._eventProcessor = eventProcessor;
            InitializeRabbitMQ();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ModuleHandle, ea) => 
            {
                Console.WriteLine("---> Event Recieved");
                var body = ea.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());
                _eventProcessor.ProcessEvent(notificationMessage);
            };
            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
            return Task.CompletedTask;
        }

        private void InitializeRabbitMQ()
        {
            var factory = new ConnectionFactory(){ HostName= _configuration["RabbitMQHost"], Port= int.Parse(_configuration["RabbitMQPort"]) };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            _channel.ExchangeDeclare(exchange: _authServiceExchangeName, type: ExchangeType.Direct);
           
            _channel.QueueBind(queue: _queueName, exchange: _authServiceExchangeName, routingKey: "");
            Console.WriteLine("--> Listening on message bus");
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
            
        }

        public override void Dispose()
        {
            Console.WriteLine("---> Message Bus Disposed");
            if(_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
            base.Dispose();
        }
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("---> RabbitMQ Connection Shutdown");
        }
    }
}
