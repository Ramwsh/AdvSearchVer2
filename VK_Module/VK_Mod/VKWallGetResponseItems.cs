using Newtonsoft.Json;
using System.Collections.Generic;

namespace VK_Module.VK_Mod
{
    public class VKWallGetResponseItems
    {
        [JsonProperty("count")]
        public int Count { get; set; }
        [JsonProperty("items")]
        public List<VKWallPost> Items { get; set; }
    }
}
