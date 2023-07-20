# GptUnityServer

## IMPORTANT

- This project is still under construction, and the instructions listed here are liable to change. Full documentation will be posted on a dedicated documentation site later on.
- Currently, the full GptUnityClient is not available on the asset store yet. Please only use the HTTP protocol for now or build a unique Unity Client using the [Unity-Net-Core-Web-Sockets repository](https://github.com/JohannesDeml/Unity-Net-Core-Networking-Sockets).


## Overview
The GptUnityServer is one half of the GptToUnity program, a tool meant to streamline the process of providing access to AI services inside the Unity Engine.

The server component performs the following functions:

- Connect to the GptUnityClient
- Access cloud and AI services on behalf of the Unity Client using ASP.Net Core
- Exchange data between Unity and the AI Service using a Shared Library of models


## Communication Protocols with Unity Client

The currently supported protocols used to communicate between Unity and the GptUnityServer are:

- TCP
- UDP
- HTTP

This uses the NetCoreServer repository to create a client-server relationship with the Unity Engine and pass information between this server and Unity. The protocol for the server can be set in two ways:
- By the `launchsettings.json` file inside the server, under the `DefaultProtocol` variable.
- Being passed into the startup `args[0]` parameter by the Unity Client.

The HTTP protocol can be freely used and accessed from Unity using UnityWebRequest. The TCP and UDP protocols can be tested by creating instances of TCP/UDP clients found in the `NetCoreServer.csproj`

The TCP and UDP Unity clients are available in the [Unity Asset Store page]() for this tool.


### Note for HTTP
The Swagger UI package is included in this server to assist with debugging the HTTP endpoints. It is disabled by default but can be re-enabled by going into `launchsettings.json` inside the `GptUnityServer.csproj` -> Properties and uncommenting the following two lines:

//"launchBrowser": true,
//"launchUrl": "swagger",


## Access Local and Cloud AI Services

This server currently has support for LocalAi Frontend Software and Cloud AI APIs. The required information to use these APIs can be filled inside the `appsettings.json` file or passed into the program on start through its `args[1]`.

For more information about LocalAi and what frontend software is, visit [Crataco's Ai Guide](https://github.com/Crataco/ai-guide).

There are four main service actions the server sets up for AI interactions:

| Service Action     | Description |
| ----------- | ----------- |
| Response      | Posts a text prompt to the endpoint and receives a text response from the AI       |
| Chat   | Posts a series of system, user, and preset messages to the AI and receives a contextual response        |
| Model List      | Gets a list of all models the AI service offers (Cloud AI Only)       |
| API Key Validation   | Checks if the AiApiKey is valid (Cloud AI Only)        |

If a particular AI service either doesn't need or doesn't support one of the four actions above, a Mock version of the service action is put in its place, which gives a preset output. All mock services can be seen in the `_MockServices` folder.


### Local AI Services

Currently supported local AI frontends are as follows:
- [Kobold AI](https://github.com/KoboldAI/KoboldAI-Client)
- [Oobabooga's Text Generation Web UI](https://github.com/oobabooga/text-generation-webui)

Simply use the corresponding service type for the frontend you would like to access.


### Cloud AI Services

All AI cloud services are handled using the AiApi service type. The details of each service are filled in using the following variables inside `appsettings.json`:

    "AiApiUrl": "URL endpoint for the chat/response for the AI service",
    "AiApiKey": "DO NOT POPULATE IN APPSETTINGS.JSON. Use secrets.json to fill this value!",
    "AiApiKeyValidationUrl": "URL to validate the API key"

These variables can be automatically populated by the GptUnityClient through the server's startup `args[2].` 

The service is meant to be generic and handle most current and future Cloud AI APIs. It currently is confirmed to work with OpenAI's API. Additional refactors and services may be created in the future to support more options.


### Unity Cloud Code Services

For production use, I recommend using the UnityCloud service type. This allows for calling Unity Cloud Code modules and scripts from the Unity Gaming Services serverless architecture, ensuring specific forms of data, like API Keys, are better secured. To use Cloud Code services, the following variables must be populated in `appsettings.json`:

    "CloudAuthToken": "PlayerAuthenticationToken",
    "UnityCloudProjectId": "The project id of your game on Unity Gaming Services",
    "CloudCodeEndpoint": "Add scripts for Cloud Code Scripts or module/ModuelName for CloudCodeModules",
    "CloudResponseFunction": "Endpoint name for the function you want to use for the response service",
    "CloudModelListFunction": "Endpoint name for the function you want to use for the model list service",
    "UnityCloudChatFunction": "Endpoint name for the function you want to use for the chat service"

These variables can be automatically populated by the GptUnityClient through the server's startup `args[2].` 

## SharedLibrary Classes with Unity

The `SharedLibrary.csproj` contains the classes and models that can be shared between this server and the GptUnityClient. This makes data conversion and translation easier between your choice of AI Service and Unity. To keep the library in sync between Unity and the server, it's recommended to use the `SharedLibrary.csproj` inside the GptUnityClient and reference that file inside the GptUnityServer solution file.


# Installation

Simply download and extract the .zip file from the releases page. 

The server is a Visual Studio project that can be launched independently from Unity for testing, and all configuration information is populated by the `appsettings.json` file.

For production, build the server and place it in the Unity game's directory. The GptUnityClient can then start the server at runtime and populate it with information dynamically.
