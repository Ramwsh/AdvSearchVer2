using Newtonsoft.Json;
using System.Collections.Generic;

namespace VK_Module.VK_Mod
{
    public class VKPhoto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("album_id")]
        public int Album_id { get; set; }

        [JsonProperty("owner_id")]
        public int Owner_id { get; set; }

        [JsonProperty("sizes")]
        public List<PhotoSize> Sizes { get; set; }        
    }
}
