using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.GptToUnity.SharedLibrary
{
    public class ModelData
    {
        public string ModelName { get; set; }
        public string ModelId { get; set; }
        public string ModelDescription { get; set; }
        // Add any other fields you want to parse from the response
    }
}
