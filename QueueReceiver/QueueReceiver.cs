using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace QueueReceiver
{
    class QueueReceiver
    {
        static ServiceBusClient client;
        static ServiceBusProcessor processor;

        static void Main(string[] args)
        {
            // Test if input arguments were supplied.
            if (args.Length == 0)
                Console.WriteLine("Usage: Program <Queue Name> <Connection String>");

            MainAsync(args[0], args[1]).GetAwaiter().GetResult();
        }

        // handle received messages
        static async Task MessageHandler(ProcessMessageEventArgs args)
        {
            DateTimeOffset msgUtcNow = DateTime.UtcNow;
            string body = args.Message.Body.ToString();
            Console.WriteLine($"RCVD: SeqNum:{args.Message.SequenceNumber} [Latency:{msgUtcNow.Subtract(args.Message.EnqueuedTime)}] Body:{body}");

            // complete the message. messages is deleted from the queue. 
            await args.CompleteMessageAsync(args.Message);
        }

        // handle any errors when receiving messages
        static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        static async Task MainAsync(string queueName, string serviceBusConnectionString)
        {
            // The Service Bus client types are safe to cache and use as a singleton for the lifetime
            // of the application, which is best practice when messages are being published or read
            // regularly.
            //

            // Create the client object that will be used to create sender and receiver objects
            client = new ServiceBusClient(serviceBusConnectionString);

            // create a processor that we can use to process the messages
            processor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions());

            try
            {
                // add handler to process messages
                processor.ProcessMessageAsync += MessageHandler;

                // add handler to process any errors
                processor.ProcessErrorAsync += ErrorHandler;

                // start processing 
                await processor.StartProcessingAsync();

                Console.WriteLine("Wait for a minute and then press any key to end the processing");
                Console.ReadKey();

                // stop processing 
                Console.WriteLine("\nStopping the receiver...");
                await processor.StopProcessingAsync();
                Console.WriteLine("Stopped receiving messages");
            }
            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
                await processor.DisposeAsync();
                await client.DisposeAsync();
            }
        }
    }
}
