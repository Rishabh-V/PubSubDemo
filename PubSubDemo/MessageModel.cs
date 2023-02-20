using Google.Cloud.PubSub.V1;

namespace PubSubDemo;

/// <summary>
/// A model for a message received from a subscription.
/// </summary>
public class MessageModel
{
    public string MessageId => Message?.MessageId;

    public PubsubMessage Message { get; set; }
}
