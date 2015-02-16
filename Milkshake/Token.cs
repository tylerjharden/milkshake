using Newtonsoft.Json;

namespace Milkshake
{
    // Represents a keyword. Associated to each product containing this keyword/token. Used for search result ranking.
    public class Token
    {
        [JsonProperty("Word")]
        public string Word { get; set; }

        public Token()
        {
            Word = "";
        }
    }
}
