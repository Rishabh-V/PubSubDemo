using System.Collections.Concurrent;
using System.Threading.Channels;

namespace PubSubDemo;

/// <summary>
/// A simple message queue implementation using System.Threading.Channels.
/// </summary>
public class MessageQueue
{
    private readonly Channel<MessageModel> _queue;
    private readonly ILogger<MessageQueue> _logger;
    private readonly ConcurrentBag<MessageModel> _messages;

    /// <summary>
    /// This will have all the messages received by the subscriber.
    /// </summary>
    public ConcurrentBag<MessageModel> Messages => _messages;

    public string LatestMessageId { get; set; }

    public MessageQueue(ILogger<MessageQueue> logger)
    {
        _logger = logger;
        _queue = Channel.CreateUnbounded<MessageModel>();
        _messages = new ConcurrentBag<MessageModel>();
    }

    /// <summary>
    /// Pushes a message to the queue.
    /// </summary>
    /// <param name="message">The message to be pushed to the queue.</param>
    /// <returns>The value task.</returns>
    public async ValueTask PushAsync(MessageModel message)
    {
        _logger.LogInformation(message: $"Pushing message {message.MessageId} to the queue.");
        await _queue.Writer.WriteAsync(message);
    }

    /// <summary>
    /// Pulls a message from the queue.
    /// </summary>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to cancel the read from the queue.</param>
    /// <returns>The <see cref="MessageModel"/>.</returns>
    /// <remarks>
    /// This method may wait indefinitely if there is no message in the queue. So, use with cancellation token.
    /// While reading the message from the queue, it will be added to the <see cref="Messages"/> collection and the <see cref="LatestMessageId"/> is set to the latest pulled message.
    /// </remarks>
    public async ValueTask<MessageModel> PullAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Pulling message from the queue");
            var data = await _queue.Reader.ReadAsync(cancellationToken);
            LatestMessageId = data.MessageId;
            Messages.Add(data);
            return data;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while pulling the message from the queue.");
            return default;
        }
    }    
}
