using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace QueueSender
{
    class QueueSender
    {
        static ServiceBusClient queueClient;
        static ServiceBusSender sender;

        static void Main(string[] args)
        {
            int numMsgs;

            if (args.Length == 0)
            {
                System.Console.WriteLine("Usage: Program <Queue Name> <Connection String> <Num of Msgs>");
            }
            
            bool test = int.TryParse(args[2], out numMsgs);
            if (!test)
            {
                Console.WriteLine("Please enter a numeric value for number of messages.");
            }

            MainAsync(args[0], args[1], numMsgs).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string queueName, string serviceBusConnectionString, int numberOfMessages)
        {
            // The Service Bus client types are safe to cache and use as a singleton for the lifetime
            // of the application, which is best practice when messages are being published or read
            // regularly.
            //
            // Create the clients that we'll use for sending and processing messages.
            queueClient = new ServiceBusClient(serviceBusConnectionString);
            sender = queueClient.CreateSender(queueName);

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
                // Use the producer client to send the batch of messages to the Service Bus queue
                await sender.SendMessagesAsync(messageBatch);
                Console.WriteLine($"A batch of {numberOfMessages} messages has been published to the queue.");
            }
            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
                await sender.DisposeAsync();
                await queueClient.DisposeAsync();
            }

            Console.WriteLine("== Press any key to exit ==");
            Console.ReadKey();
        }
    }
}
