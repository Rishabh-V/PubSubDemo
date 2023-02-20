using Google.Cloud.PubSub.V1;

namespace PubSubDemo;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;

        // Read the settings.
        var credentialsPath = configuration.GetValue<string>("CredentialsPath");
        var topicId = configuration.GetValue<string>("TopicId");
        var projectId = configuration.GetValue<string>("ProjectId");
        var subscriptionId = configuration.GetValue<string>("SubscriptionId");

        TopicName topicName = TopicName.FromProjectTopic(projectId, topicId);
        SubscriptionName subscriptionName = SubscriptionName.FromProjectSubscription(projectId, subscriptionId);

        if (!string.IsNullOrWhiteSpace(credentialsPath))
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialsPath);
        }

        // Add services to the container.
        builder.Services.AddRazorPages();

        // Add the message queue as a singleton.
        builder.Services.AddSingleton<MessageQueue>();
        
        // Add the hosted services.
        builder.Services.AddHostedService<SubscriberService>();

        // Add the Pub/Sub clients as singleton.
        builder.Services.AddPublisherClient(topicName);
        builder.Services.AddSubscriberClient(subscriptionName);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapRazorPages();

        app.Run();
    }
}