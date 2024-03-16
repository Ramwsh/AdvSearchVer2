using Newtonsoft.Json;

namespace VK_Module.VK_Mod
{
    public class VKWallAttachment
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("photo")]
        public VKPhoto Photo { get; set; }
    }
}
