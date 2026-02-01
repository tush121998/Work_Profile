using System;

namespace Shop.MessageBus;

public interface IMessageBus
{
    Task PublishMessageAsync<T>(T message, string topicName);
}
