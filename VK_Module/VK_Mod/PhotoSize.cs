using Newtonsoft.Json;

namespace VK_Module.VK_Mod
{
    public class PhotoSize
    {
        [JsonProperty("height")]
        public int Height { get; set; }
        [JsonProperty("width")] 
        public int Width { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
