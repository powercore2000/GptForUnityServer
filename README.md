# GptUnityServer

## IMPORTANT

- This project is still under construction, and the instructions listed here are liable to change. Full documentation will be posted on a dedicated documentation site later on.


## Overview
Have you wanted to get the power of local and cloud Ai inside of unity to help build your games? Well look no further! The GptForUnity program comes in two parts:

GptForUnityServer a middleware tool meant to host TCP/UDP servers to connect Unity with Ai service APIs (OobaUi,OpenAi, ect) 
GptForUnityClient, a user interface and client meant to recive and handle prompting the Ai services


The server component performs the following functions:

- Connect to the GptUnityClient
- Access cloud and local Ai services on behalf of the Unity Client using ASP.Net Core
- Exchange data between Unity and the AI Services by using a Shared Library of models


## Communication Protocols with Unity Client

The currently supported protocols used to communicate between Unity and the GptUnityServer are:

- TCP
- UDP
- HTTP (WIP)

## TCP and UDP Notes

This uses the NetCoreServer repository to create a client-server relationship with the Unity Engine and pass information between this server and the Unity Client. The protocol for the server can be set in two ways:
- By the `launchsettings.json` file inside the server, under the `DefaultProtocol` variable.
- Being passed into the startup `args[0]` parameter by the Unity Client.

The HTTP protocol can be freely used and accessed from Unity using UnityWebRequest. The TCP and UDP protocols can be tested by using the `GptUnityServerTests.csproj`


### HTTP Notes
The Swagger UI package is included in this server to assist with debugging the HTTP endpoints. It is disabled by default but can be re-enabled by going into `launchsettings.json` inside the `GptUnityServer.csproj` -> Properties and uncommenting the following two lines:

//"launchBrowser": true,
//"launchUrl": "swagger",


## Access Local and Cloud AI Services

This server currently has support for LocalAi Frontend Software and Cloud AI APIs. The required information to use these APIs can be filled inside the `appsettings.json` file or passed into the program on start through its `args[1]`.

For more information about LocalAi and what frontend software is, visit [Crataco's Ai Guide](https://github.com/Crataco/ai-guide).

The server interacts with the Ai backend through services. The currently supported services are:

| Service Action     | Description |
| ----------- | ----------- |
| Response      | Posts a text prompt to the endpoint and receives a text response from the AI       |
| Chat   | Sends context, personality, and chat history, between the user, and the AI service to simulate conversion|
| Model List      | Gets a list of all models the AI service offers (Cloud AI Only)       |
| API Key Validation   | Checks if the AiApiKey is valid (Cloud AI Only)        |

If a particular AI service either doesn't need or doesn't support one of the four actions above, a Mock version of the service action is put in its place, which gives a preset output. All mock services can be seen in the `_MockServices` folder.


### Local AI Services

Currently supported local AI frontends are as follows:
- [Kobold AI](https://github.com/KoboldAI/KoboldAI-Client)
- [Oobabooga's Text Generation Web UI](https://github.com/oobabooga/text-generation-webui)

To use these services, simply have them running in the background and the GptForUnityServer will be able to connect with them

#### Use with GptForUnityClient
To get the client to auto activate these services, find their `start.bat` file's location using the find bat file browser bar on the login screen. The directory will save to player prefs and be loaded from there.


### Cloud AI Services

All AI cloud services are handled using the AiApi service type. The details of each service are filled in using the following variables inside `appsettings.json`:

    "AiApiUrl": "URL endpoint for the chat/response for the AI service",
    "AiApiKey": "DO NOT POPULATE IN APPSETTINGS.JSON. Use secrets.json to fill this value!",
    "AiApiKeyValidationUrl": "URL to validate the API key"

These variables can be automatically populated by the GptUnityClient through the server's startup `args[2].` 

The service is meant to be generic and handle most current and future Cloud AI APIs. It currently is confirmed to work with OpenAI's API. Additional refactors and services may be created in the future to support more options.


### Unity Cloud Code Services


For production use of Cloud Ai's, I recommend using the UnityCloudCode service type. This allows for calling Unity Cloud Code modules and scripts from the Unity Gaming Services serverless architecture, ensuring specific forms of data, like API Keys, are better secured. To use Cloud Code services, the following variables must be populated in `appsettings.json`:

    "CloudAuthToken": "PlayerAuthenticationToken",
    "UnityCloudProjectId": "The project id of your game on Unity Gaming Services",
    "CloudCodeEndpoint": "Add scripts for Cloud Code Scripts or module/ModuelName for CloudCodeModules",
    "CloudResponseFunction": "Endpoint name for the function you want to use for the response service",
    "CloudModelListFunction": "Endpoint name for the function you want to use for the model list service",
    "UnityCloudChatFunction": "Endpoint name for the function you want to use for the chat service"

These variables can be automatically populated by the GptUnityClient through the server's startup `args[2].` 

## SharedLibrary Classes with Unity

The `SharedLibrary.csproj` contains the classes and models that can be shared between this server and the GptUnityClient. This makes data conversion and translation easier between your choice of AI Service and Unity. To keep the library in sync between Unity and the server, it's recommended to use the `SharedLibrary.csproj` inside the GptUnityClient and reference that file inside the GptUnityServer solution file.


## Prompt Construction

The GptForUnityClient has two ways of building its prompts for interaction with an Ai backend. 


| Prompt Construction Type    | Description |
| ----------- | ----------- |
| File      | Load both parameter settings and character/prompt info from json files under the settings button       |
| Ui   | Build the character/prompt information and parameter settings from the UI options in the GptPlayground scene    |

# Installation

Download the `GptForUnityServer.zip` and extract the file from the releases page in side of your Unity project's directory. (It should sit next to the Assets and Libary folder). The GptUnityClient can then start the server at runtime and populate it with information dynamically.

Download the `GptForUnityClient.unitypackadge` and add that to your unity project. 

Load up the `GptPlayground.unity` Scene

Find the `Start.bat` file of your local Ai service using the Bat File Search bar if using local AI's

Select your desired Protocol, Service Type, and Prompt Construction

Hit Login and access the playground!

The server is a Visual Studio project that can be launched independently from Unity for testing, and all configuration information is populated by the `appsettings.json` file.

# Final Notes
- I reccommend TCP protocol for character based chats, as the prompts are so big, packet loss can occure with UDP
- HTTP does not use the Client Ui workflow, you would need to write your own UnityWebRequests


# Contributions

-This wouldnt be possible without the [Unity-Net-Core-Web-Sockets repository](https://github.com/JohannesDeml/Unity-Net-Core-Networking-Sockets).
-Big thanks to [The Unity Standalone File Brower](https://github.com/gkngkc/UnityStandaloneFileBrowser)
