# GptUnityServer

## IMPORTANT

- This project is still under construction, and the instructions listed here are liable to change. Full documentation will be posted on a dedicated documentation site later on.


## Overview
Have you wanted to get the power of local and cloud Ai inside of Unity to help build your games? Well, look no further! The GptForUnity program comes in two parts:

GptForUnityServer is a middleware tool meant to host TCP/UDP servers to connect Unity with Ai service APIs (OobaUi,OpenAi, ect) 
GptForUnityClient, a user interface, and client meant to receive and handle prompting the Ai services


The server component performs the following functions:

- Connect to the GptUnityClient
- Access cloud and local Ai services on behalf of the Unity Client using ASP.Net Core
- Exchange data between Unity and the AI Services by using a Shared Library of models

# Installation 

Download the `GptForUnityServer.zip` and extract the file from the releases page inside your Unity project's directory. (It should sit next to the Assets and Libary folder). The GptUnityClient can then start the server at runtime and populate it with information dynamically.

Download the `GptForUnityClient.unitypackadge` and add that to your unity project. 

Load up the `GptPlayground.unity` Scene

Find the `Start.bat` file of your local Ai service using the Bat File Search bar if using local AI's

Select your desired Protocol, Service Type, and Prompt Construction

Hit Login and access the playground!

The server is a Visual Studio project that can be launched independently from Unity for testing, and all configuration information is populated by the `appsettings.json` file.

# Final Notes
- I recommend TCP protocol for character-based chats, as the prompts are so big, packet loss can occur with UDP
- HTTP does not use the Client Ui workflow, you would need to write your own UnityWebRequests


# Contributions

-This wouldn't be possible without the [Unity-Net-Core-Web-Sockets repository](https://github.com/JohannesDeml/Unity-Net-Core-Networking-Sockets).
-Big thanks to [The Unity Standalone File Brower](https://github.com/gkngkc/UnityStandaloneFileBrowser)
