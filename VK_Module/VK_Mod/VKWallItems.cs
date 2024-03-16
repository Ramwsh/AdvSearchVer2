using Newtonsoft.Json;
using System.Collections.Generic;

namespace VK_Module.VK_Mod
{
    public class VKWallItems
    {
        [JsonProperty("items")]
        public List<VKWallPost> Items { get; set; }
    }
}
