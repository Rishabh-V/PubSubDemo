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

If the appsettings.json file is updated correctly, the application should load correctly and we should see a landing page in which a textbox and button should appear where the user can enter a text message and click on submit button and the submitted message will appear with the published message ID in the bottom of the screen. The high level flow can be seen as shown next:


## Working
- Both `PublisherClient` and `SubscriberClient` are registerd as singleton in the DI container using the new extension methods available in the library.
- 



## Working 
