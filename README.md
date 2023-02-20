# PubSub Demo
This repository demonstrates how to use the Google Cloud `PublisherClient` and `SubscriberClient` in a Dependency Injection (DI) friendly way using the PubSub .NET client library.

## Prerequisites
Before running the sample code, you need to have:
 - A GCP project with the PubSub API enabled.
 - The .NET Core SDK version 6.0 or higher installed on your machine.

## Setup

- Clone the repository.
- Open the solution in Visual Studio 2022 or your favorite IDE.
- Open the appsettings.json file and replace the placeholders with your GCP Project ID, Topic ID, Subscription ID and the Credentials Json Path (optional if Auth is taken care)
- There is a NuGet package named `Google.Cloud.PubSub.V1` v3.4.0 required for this project to run which has not been released yet. This package has been built from local and is placed in the project folder. This package should be copied to a folder and that should be added as one of the NuGet package sources so that the package can be restored successfully.
- Build the solution.
- Run the sample app.

## Usage
If the package restores correctly, the solution should build successfully.

If the appsettings.json file is updated correctly, the application should load correctly and we should see a landing page in which a textbox and button should appear where the user can enter a text message and click on submit button and the submitted message will appear with the published message ID in the bottom of the screen. The high level flow can be seen [here](https://user-images.githubusercontent.com/15943060/220071996-7144ed9c-455e-4d01-9480-9970b19a64bb.mp4) or below.

<img src="https://user-images.githubusercontent.com/15943060/220071996-7144ed9c-455e-4d01-9480-9970b19a64bb.mp4"/>

## Working
- Both `PublisherClient` and `SubscriberClient` are registerd as singleton in the DI container using the new extension methods available in the library.
- There is a hosted background service named `SubscriberService` that starts the singleton `SubscriberClient`. The `SubscriberClient` listens to the subscription specified in the appsettings.json for any new messages.
- There is a singleton `MessageQueue` based on `Channel<T>` and hence can be used concurrently. This queue holds the message received from the `SubscriberClient`
- When the user enters the message in the textbox and clicks on the Submit button, the singleton `PublisherClient` publishes the message to the specified Topic.
- The singleton `SubscriberClient` running in the background `SubscriberService` picks the message from the specified `Subscription`and adds it to the `MessageQueue`
- The page then picks up the message from the `MessageQueue` and displays it on the page. 

_Note_: This is just a demo application. In this sample app, the message is picked up from the queue at the same time when the publisher publishes a message. In case there are multiple instances of the app running then a newly published message may not display in the page until a message is published from that instance. That can be fixed by various ways including a timer based refresh logic which can refresh the page every n seconds or so. This however is not the scope or intention of the app.  


