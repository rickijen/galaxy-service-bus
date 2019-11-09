﻿using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace TopicSender
{
    class TopicSender
    {
        static ITopicClient topicClient;
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
        static async Task MainAsync(string TopicName, string ServiceBusConnectionString, int numberOfMessages)
        {
            topicClient = new TopicClient(ServiceBusConnectionString, TopicName);

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
    }
}
