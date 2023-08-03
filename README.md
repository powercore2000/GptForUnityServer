# GptUnityServer

## IMPORTANT

- This project is still under construction, and the instructions listed here are liable to change. Check the wiki on this github for information


## Overview
Have you wanted to get the power of local and cloud Ai inside of unity to help build your games? Well look no further! The GptForUnity program comes in two parts:

GptForUnityServer a middleware tool meant to host TCP/UDP servers to connect Unity with Ai service APIs (OobaUi,OpenAi, ect) 
GptForUnityClient, a user interface and client meant to recive and handle prompting the Ai services


The server component performs the following functions:

- Connect to the GptUnityClient
- Access cloud and local Ai services on behalf of the Unity Client using ASP.Net Core
- Exchange data between Unity and the AI Services by using a Shared Library of models



# Final Notes
- I reccommend TCP protocol for character based chats, as the prompts are so big, packet loss can occure with UDP
- HTTP does not use the Client Ui workflow, you would need to write your own UnityWebRequests


# Contributions

-This wouldnt be possible without the [Unity-Net-Core-Web-Sockets repository](https://github.com/JohannesDeml/Unity-Net-Core-Networking-Sockets).
-Big thanks to [The Unity Standalone File Brower](https://github.com/gkngkc/UnityStandaloneFileBrowser)
