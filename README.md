# Chatnik - simple chat

Simple application based on zeroMQ (PUB-SUB) protocol for chat

## How to test Server application
Go to the [/dist/Server](https://github.com/aco228/VMode.Chatnik/tree/main/dist/Server) folder, and though terminal execute `Chatnik.ServerApplication.exe [PublisherPort] [SubscriberPort]`
If you dont specify the port number, it will use default ones configured in [appsettings.json](https://github.com/aco228/VMode.Chatnik/blob/main/src/Chatnik.ClientApplication/appsettings.json) (publisherPort=23452, subscriberPort=23451)
For closing app, click `Ctrl-C`, or `Command-C`

## How to test client application
Go to the [/dist/Client](https://github.com/aco228/VMode.Chatnik/tree/main/dist/Client) and open `Chatnik.ClientApplication.exe` after which you will get form like this
![image](https://user-images.githubusercontent.com/35331284/144751473-0a112b56-3f27-4ad1-8e3b-1d197e2b4545.png)
Enter ports you defined in the server, and username you want to use in the chat. After all information are set, you will be shown main chat form
![image](https://user-images.githubusercontent.com/35331284/144751511-f2b3d94d-115d-45e4-8204-efd7c4a329c7.png)



# Shared architecture

As client and server application share most of the logic, that logic is defined and implemeted in [Chatnik.Shared](https://github.com/aco228/VMode.Chatnik/tree/main/src/Chatnik.Shared) project.

One of the main building blocks is [IBackgroundRunner.cs](https://github.com/aco228/VMode.Chatnik/blob/main/src/Chatnik.Shared/Interfaces/IBackgroundRunner.cs) which is used to create separate thread and execute defined logic. 

With `IBackgroundRunner` we can implement [IMessageListener.cs](https://github.com/aco228/VMode.Chatnik/blob/main/src/Chatnik.Shared/Interfaces/IMessageListener.cs), background process which can get message, and pass it to specific message processor ([IMessageProcessor.cs](https://github.com/aco228/VMode.Chatnik/blob/main/src/Chatnik.Shared/Interfaces/IMessageProcessor.cs))

`IMessageProcessor` is listening for its specific topic, and it is used to process specific type of messages for that topic. Main idea is to receive all messages that is client subscribed to, get and desterilize that message, and pass it to the specific `IMessageProcessor` for further processing.

All messages inherit from [IMessage.cs](https://github.com/aco228/VMode.Chatnik/blob/main/src/Chatnik.Shared/Interfaces/IMessage..cs) and there are two types:
- [ITransferMessage.cs](https://github.com/aco228/VMode.Chatnik/blob/main/src/Chatnik.Shared/Interfaces/ITransferMessage.cs) used for sending messages through the network, and for end use in `IMessageProcessor`. All data transfer objects are inherited from this base type, and all of their properties are serialized into NetMQMessage frames.
- [IReceiveMessage.cs](https://github.com/aco228/VMode.Chatnik/blob/main/src/Chatnik.Shared/Interfaces/IReceiveMessage.cs) is used to desterilize message received, and transfer it into `ITransferMessage`

All socket implementation is inherited from [IChatnikSocket.cs](https://github.com/aco228/VMode.Chatnik/blob/main/src/Chatnik.Shared/Interfaces/IChatnikSocket.cs), and there are two types that act as a wrapper around NetMQ socket implementation `INetMQSocket`
- [IChatnikPublisherSocket.cs](https://github.com/aco228/VMode.Chatnik/blob/main/src/Chatnik.Shared/Interfaces/IChatnikPublisherSocket.cs) is a wrapper used for publishing messages and surrounding logic
- [IChatnikSubscriberSocket.cs](https://github.com/aco228/VMode.Chatnik/blob/main/src/Chatnik.Shared/Interfaces/IChatnikSubscriberSocket.cs) is wrapper used for receiving messages and surrounding logic.

## Server implementation

Server implementation is fairly simple. It is using `IMessageListener` with single message processor [ReceiveAndReturnMessageProcessor.cs](https://github.com/aco228/VMode.Chatnik/blob/main/src/Chatnik.ServerApplication/MessageProcessors/ReceiveAndReturnMessageProcessor.cs) that will receive its original message, and send it back to the client.

## Client application implementation

For client implementation WindowsForms has been used with `MVP` design pattern for easier testing's.  It contains two forms, 
- UserLogin (simple form for setting contract values such as `RemoteAddress`, port number for publisher and subscriber, and username).
- ChatForm, used for receive and send chat messages

Client form is responsible for checking if server application is alive at the moment. For that it is using simple heartbeat mechanism. Idea is to send hearthbeat message to the server, and check will server respond to that message in time. For this implementation we have [HearthbeatService.cs](https://github.com/aco228/VMode.Chatnik/blob/main/src/Chatnik.ClientApplication.Core/Services/HearthbeatService.cs) that will publish message, and keep track if we got response message in time, and message processor [HearthbeatMessageProcessor.cs](https://github.com/aco228/VMode.Chatnik/blob/main/src/Chatnik.ClientApplication.Core/MessageProcessors/HearthbeatMessageProcessor.cs) that will fire event when ever we receive response from the server.

If server do not respond in specific type, hearthbeat service will fire an event, and messages will be shown on `ChatForm`
![image](https://user-images.githubusercontent.com/35331284/144752284-a27132fa-c7d0-4023-be62-b51e0ed7238c.png)
Also, when server start again to respond to messages, new message will be shown
![image](https://user-images.githubusercontent.com/35331284/144752305-d9864b27-ff28-4cc2-9651-f2d4600b8fe3.png)


Whenever we receive any of the chat messages from the server, they will go though [ChatMessageProcessor.cs](https://github.com/aco228/VMode.Chatnik/blob/main/src/Chatnik.ClientApplication.Core/MessageProcessors/ChatMessageProcessor.cs), which will fire event.

Chat messages are defined with [ChatMessage.cs](https://github.com/aco228/VMode.Chatnik/blob/main/src/Chatnik.Shared/Models/ChatMessage.cs), and all of them have specific type [ChatMessageType.cs](https://github.com/aco228/VMode.Chatnik/blob/main/src/Chatnik.Shared/Models/ChatMessageType.cs)

As soon as user enters the `ChatForm`, client will send chat message with the type `UserJoined`, and when user closes `ChatForm`, app will fire another message with type `UserJoined`.

## Ideas to improve application

- As client application, server can also keep track of active users
- Adding feature that users can create public/private channels, (or just chat channels between them), with ability to have a list of all active users in specific channel
- End-to-end encryption, better signup/login processes, for better security
- Logging for better bug tracking
- Instead of winforms, use better ui library that can potentially be used in different environments instead of just windows
