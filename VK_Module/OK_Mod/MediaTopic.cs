using System.Collections.Generic;

namespace VK_Module.OK_Mod
{
    public class MediaTopic
    {
        public string Text { get; set; }
        public List<string> PhotoUrls { get; set; }

        public MediaTopic()
        {
            PhotoUrls = new List<string>();
        }
    }
}
