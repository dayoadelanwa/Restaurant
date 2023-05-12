using Mango.MessageBus;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Mango.Services.PaymentAPI.RabbitMQSender
{
    public class RabbitMQPaymentMessageSender : IRabbitMQPaymentMessageSender
    {
        private readonly string _hostname;
        private readonly string _password;
        private readonly string _username;
        private IConnection _connection;
        //private const string ExchangeName = "PublishSubscribePaymentUpdate_Exchange";
        private const string ExchangeName = "DirectPaymentUpdate_Exchange";
        private const string PaymentEMailUpdateQueueName = "PaymentEMailUpdateQueueName";
        private const string PaymentOrderUpdateQueueName = "PaymentOrderUpdateQueueName";

        public RabbitMQPaymentMessageSender()
        {
            _hostname= "localhost";
            _password= "guest";
            _username= "guest";
        }
        public void SendMessage(BaseMessage baseMessage)
        {
            if (ConnectionExists())
            {
                using var channel = _connection.CreateModel();
                channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, durable: false);
                channel.QueueDeclare(queue: PaymentEMailUpdateQueueName, false, false, false, arguments: null);
                channel.QueueDeclare(queue: PaymentOrderUpdateQueueName, false, false, false, arguments: null);

                channel.QueueBind(PaymentEMailUpdateQueueName, exchange: ExchangeName, "PaymentEmail");
                channel.QueueBind(PaymentOrderUpdateQueueName, exchange: ExchangeName, "PaymentOrder");

                var json = JsonConvert.SerializeObject(baseMessage);
                var body = Encoding.UTF8.GetBytes(json);
                //channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);

                channel.BasicPublish(exchange: ExchangeName, "PaymentEmail", basicProperties: null, body: body);
                channel.BasicPublish(exchange: ExchangeName, "PaymentOrder", basicProperties: null, body: body);
            }
        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostname,
                    UserName = _username,
                    Password = _password,
                };
                _connection = factory.CreateConnection();

            }
            catch(Exception)
            {
                //log exception
            }
        }

        private bool ConnectionExists()
        {
            if (_connection != null)
            {
                return true;
            }
            CreateConnection();
            return _connection != null;
        }
    }
}
