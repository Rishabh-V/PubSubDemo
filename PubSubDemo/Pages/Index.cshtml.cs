using Google.Cloud.PubSub.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Concurrent;

namespace PubSubDemo.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly PublisherClient _publisherClient;
    private readonly MessageQueue _queue;

    public ConcurrentBag<MessageModel> Messages => _queue.Messages;

    public string LatestMessageId => _queue.LatestMessageId;

    public IndexModel(PublisherClient publisherClient, MessageQueue queue, ILogger<IndexModel> logger)
    {
        _publisherClient = publisherClient;
        _queue = queue;
        _logger = logger;
    }

    public async Task<IActionResult> OnPostAsync(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            _logger.LogInformation($"The message \"{message}\" submitted is null, empty or whitespace. Returning the page in previous state.");
            return Page();
        }

        var messageID = await _publisherClient.PublishAsync(message);
        _logger.LogInformation($"Published message {messageID}: {message}");
        // Message is already pulled by subscriber client. Pull it so that it is added to list for display.
        // This is fine for demo app. If needed, we can also do a timer based service, which refreshes data every n seconds.
        await _queue.PullAsync(CancellationToken.None);
        return Page();
    }
}