using Newtonsoft.Json;
using System.Collections.Generic;


namespace VK_Module.VK_Mod
{
    public class VKWallPost
    {
        [JsonProperty("from_id")]
        public string From_Id { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("date")]
        public int Date { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("attachments")]
        public List<VKWallAttachment> Attachments { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return Text == ((VKWallPost)obj).Text;
        }

        public override int GetHashCode()
        {
            return Text.GetHashCode();
        }
    }
}
