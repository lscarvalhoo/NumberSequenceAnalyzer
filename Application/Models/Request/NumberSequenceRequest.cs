using System.Text.Json.Serialization;

namespace Application.Models.Request
{
    public class NumberSequenceRequest
    {
        [JsonPropertyName("valores")]
        public List<int>? Values { get; set; }
    }
}
