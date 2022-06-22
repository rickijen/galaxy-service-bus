![Diagram](https://github.com/rickijen/galaxy-service-bus/blob/master/sb-topics-01.png)
# galaxy-service-bus: Measure latency with competing consumers on a subscription to topic #

1. Please use the 2 powershell scripts to launch concurrent consumers.
2. Launch the message sender with the .Net Core sender executable and pass in the connection string as command line arg.
3. The average latency (delta between enqueued time and the time when message received by consumer) is averaged at 0.04 second.
4. Note: Updated this project to use the latest SDK with .NET6. Also, the senders are sending in batch mode instead of sending one by one. So the latency time in the result is actually the time since the message was enqueued. To calculate the latency between enqueued time and the time when message received by consumer, simply get the delta of two adjacent timestamps.

![Diagram](https://github.com/rickijen/galaxy-service-bus/blob/master/Test%20result.png)
