using System.Text.Json.Serialization;

namespace VK_Module.OK_Mod
{
    public class PhotoIdes
    {
        [JsonPropertyName("photo_ids")]
        public string[] photo_ids { get; set; }
        [JsonPropertyName("upload_url")]
        public string upload_url { get; set; }
    }
}
