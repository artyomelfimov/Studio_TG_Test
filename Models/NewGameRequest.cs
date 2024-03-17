using System.ComponentModel;
using System.Text.Json.Serialization;

namespace StudioTG_Test.Models
{
    public class NewGameRequest
    {
        [DefaultValue(10)]
        public int Width { get; set; }
        [DefaultValue(10)]
        public int Height { get; set; }
        [DefaultValue(10)]
        [JsonPropertyName("mines_count")]
        public int MinesCount { get; set; } 
    }
}
