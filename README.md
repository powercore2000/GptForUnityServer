# GptToUnityServer

The GptToUnityServer is one half the GptToUnity program, a tool meant to streamline the process of providong access to Ai services inside of the Unity Engine


The server component performs the following functions:

* Connect to the Unity Engine client
* Access cloud and Ai services on behalf on the Unity Client using ASP.Net Core
* Exchange data between Unity and the Ai Service using a SharedLibrary of models



##Communicate With Unity

The currently supported protcols used to communicate between Unity and the GptToUnityServer are:

*TCP
*UDP
*HTTP

This uses the NetCoreServer reposioty to create a client server relationship with the Unity Engine and pass information between this server and Unity

##Access Local and Cloud Ai Services

This server current has support for LocalAi Frontend Software, and Cloud Ai APIs.

For more information about LocalAi's and what frontend software is go here: https://github.com/Crataco/ai-guide

##Create a Shared Model Library With Unity

Uses a Shared Model .dll which contains the same classes and data models between this server and the Unity Engine Client. For the purposes of making data modification, and transference between the Ai Services and Unity easier
