
using System;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SharedLibrary
{
    public class AiResponse
    {
        public string Message { get; set; }

        public string JsonRaw { get; set; }

        public AiResponse(string _rawResponse, string _message)
        {

            JsonRaw = _rawResponse;

            Message = _message;


        }
    }

}
