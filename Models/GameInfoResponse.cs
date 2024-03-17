using System.Text.Json.Serialization;

namespace StudioTG_Test.Models
{
   
        public class GameInfoResponse
        {

        [JsonPropertyName("game_id")]
            public Guid GameID { get; set; }
            public bool Completed { get; set; }
            public  List<List<string>> Field { get; set; }
        }
 

}
