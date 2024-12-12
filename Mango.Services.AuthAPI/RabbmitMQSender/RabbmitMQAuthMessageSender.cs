using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Mango.Services.AuthAPI.RabbitMQSender
{
    public class RabbmitMQAuthMessageSender : IRabbmitMQAuthMessageSender
    {
        private readonly string _hostName;
        private readonly string _username;
        private readonly string _password;
        private IConnection _connection;
        public RabbmitMQAuthMessageSender()
        {
            _hostName= "localhost";
            _username= "guest";
            _password= "guest";
        }

        public  async void SendMessage(object message, string queueName)
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostName,
                Password = _password,
                UserName = _username,
            };
            
            _connection = await factory.CreateConnectionAsync();
            using var channel = _connection.CreateChannelAsync();
            channel.QueueDeclare(queueName);
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            channel.BasicPublish(ExchangeType:"",routingKey:queueName, body:body);
            
           
            
        }
    }
}
