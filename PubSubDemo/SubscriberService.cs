using Google.Cloud.PubSub.V1;

namespace PubSubDemo;

/// <summary>
/// A service that runs in the background and receives messages from a subscription.
/// </summary>
public class SubscriberService : BackgroundService
{
    private readonly SubscriberClient _subscriberClient;
    private readonly ILogger<SubscriberService> _logger;
    private readonly MessageQueue _queue;

    public SubscriberService(SubscriberClient subscriberClient, MessageQueue queue, ILogger<SubscriberService> logger)
    {
        _subscriberClient = subscriberClient;
        _queue = queue;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await _subscriberClient.StartAsync(async (msg, token) =>
            {
                using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken, token);
                _logger.LogInformation($"Received message {msg.MessageId}: {msg.Data}");
                await _queue.PushAsync(new MessageModel { Message = msg });
                return await Task.FromResult(SubscriberClient.Reply.Ack);
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while receiving message.");
        }
    }
}
