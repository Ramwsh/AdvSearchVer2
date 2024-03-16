using Newtonsoft.Json;

namespace VK_Module.VK_Mod
{
    public class VKWallGetResponse
    {
        [JsonProperty("response")]        
        public VKWallGetResponseItems Response { get; set; }
    }
}
