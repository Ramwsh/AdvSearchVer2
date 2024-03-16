using Newtonsoft.Json;
using System.Collections.Generic;

namespace VK_Module.VK_Mod
{
    public class VKGroupResponse
    {
        [JsonProperty("response")]
        public List<VKGroup> Response { get; set; }
    }
}
