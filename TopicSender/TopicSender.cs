using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace TopicSender
{
    class TopicSender
    {
        static ServiceBusClient topicClient;
        static ServiceBusSender sender;
        static void Main(string[] args)
        {
            int numMsgs;

            if (args.Length == 0)
            {
                System.Console.WriteLine("Usage: Program <Topic Name> <Connection String> <Num of Msgs>");
            }

            bool test = int.TryParse(args[2], out numMsgs);
            if (!test)
            {
                Console.WriteLine("Please enter a numeric value for number of messages.");
            }

            MainAsync(args[0], args[1], numMsgs).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string topicName, string serviceBusConnectionString, int numberOfMessages)
        {
            // The Service Bus client types are safe to cache and use as a singleton for the lifetime
            // of the application, which is best practice when messages are being published or read
            // regularly.
            //
            // Create the clients that we'll use for sending and processing messages.
            topicClient = new ServiceBusClient(serviceBusConnectionString);
            sender = topicClient.CreateSender(topicName);

            // create a batch 
            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

            for (int i = 1; i <= numberOfMessages; i++)
            {
                // try adding a message to the batch
                if (!messageBatch.TryAddMessage(new ServiceBusMessage($"Message {i}")))
                {
                    // if it is too large for the batch
                    throw new Exception($"The message {i} is too large to fit in the batch.");
                }
            }

            try
            {
                // Use the producer client to send the batch of messages to the Service Bus topic
                await sender.SendMessagesAsync(messageBatch);
                Console.WriteLine($"A batch of {numberOfMessages} messages has been published to the topic.");
            }
            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
                await sender.DisposeAsync();
                await topicClient.DisposeAsync();
            }

            Console.WriteLine("Press any key to end the application");
            Console.ReadKey();
        }

/*
        static async Task MainAsync(string topicName, string serviceBusConnectionString, int numberOfMessages)
        {
            topicClient = new TopicClient(serviceBusConnectionString, topicName);

            Console.WriteLine("======================================================");
            Console.WriteLine("Press ENTER key to exit after sending all the messages.");
            Console.WriteLine("======================================================");

            // Send messages.
            await SendMessagesAsync(numberOfMessages);

            Console.ReadKey();

            await topicClient.CloseAsync();
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
                    await topicClient.SendAsync(message);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }
*/        
    }
}
