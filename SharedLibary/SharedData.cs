using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    /// <summary>
    /// string keywords to detect in TCP/UDP messages to identify non standard chat information
    /// </summary>
    /// [ComVisible(true)]
    public static class CommunicationData
    {
        /// <summary>
        /// Keyword to run the Api Key Validation service and return if the service was successful or not
        /// </summary>
        public static string InitalizationIdText { get; private set; } = "//INITIALIZATION-MESSAGE//";
    }
}
