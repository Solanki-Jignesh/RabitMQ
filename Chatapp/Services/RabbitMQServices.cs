namespace Chatapp;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;
using System;
using System.Text;

public class RabbitMQService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private const string QueueName = "chatQueue";

    public RabbitMQService()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost", // Change if using a remote RabbitMQ server
            UserName = "guest",
            Password = "guest"
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        
        // Declare Queue
        _channel.QueueDeclare(queue: QueueName,
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);
    }

    public void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(exchange: "",
                             routingKey: QueueName,
                             basicProperties: null,
                             body: body);
    }

    public void Dispose()
    {
        _channel.Close();
        _connection.Close();
    }
}
