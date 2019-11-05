using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace galaxy_service_bus
{
    class Sender
    {
        const string ServiceBusConnectionString = "Endpoint=sb://galaxy.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=gbyeXkuFjLcmC2d4YyQZQ+SbWMje0af88qxHBdMGUks=";
        const string QueueName = "queue02";
        static IQueueClient queueClient;

        static void Main(string[] args)
        {
            int num;

            if (args.Length == 0)
            {
                System.Console.WriteLine("Please enter a numeric argument.");
            }
            
            bool test = int.TryParse(args[0], out num);
            if (!test)
            {
                Console.WriteLine("Please enter a numeric value for number of messages.");
            }

            MainAsync(num).GetAwaiter().GetResult();
        }
        static async Task MainAsync(int num)
        {
            int numberOfMessages = num;
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);

            Console.WriteLine("======================================================");
            Console.WriteLine("Press ENTER key to exit after sending all the messages.");
            Console.WriteLine("======================================================");

            // Send messages.
            await SendMessagesAsync(numberOfMessages);

            Console.ReadKey();

            await queueClient.CloseAsync();
        }
        static async Task SendMessagesAsync(int numberOfMessagesToSend)
        {
            try
            {
                for (var i = 0; i < numberOfMessagesToSend; i++)
                {
                    // Create a new message to send to the topic.
                    string messageBody = $"Message {i}";
                    var message = new Message(Encoding.UTF8.GetBytes(messageBody));

                    // Write the body of the message to the console.
                    Console.WriteLine($"Sending message: {messageBody}");

                    // Send the message to the topic.
                    await queueClient.SendAsync(message);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }

    }
}
