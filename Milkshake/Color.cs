using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Milkshake
{
    internal class Color
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Id")]
        public Guid Id { get; set; }

        [JsonProperty("Alternatives")]
        public List<String> Alternatives { get; set; }
    }
}
