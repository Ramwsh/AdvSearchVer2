using Newtonsoft.Json;

namespace VK_Module.VK_Mod
{
    public class VKGroup
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
