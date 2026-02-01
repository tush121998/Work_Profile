namespace Shop.MessageBus;

public class MessageBus : IMessageBus
{
    public async Task PublishMessageAsync<T>(T message, string topicName)
    {
        // Implementation for publishing message to the message bus
        
    }
}
