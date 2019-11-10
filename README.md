![Diagram](https://github.com/rickijen/galaxy-service-bus/blob/master/sb-topics-01.png)
# galaxy-service-bus: Measure latency with competing consumers on a subscription to topic #

1. Please use the 2 powershell scripts to launch concurrent consumers.
2. Launch the message sender with the .Net Core sender executable and pass in the connection string as command line arg.
3. The average latency (delta between enqueued time and the time when message received by consumer) is 0.04 second.

![Diagram](https://github.com/rickijen/galaxy-service-bus/blob/master/Test%20result.png)
