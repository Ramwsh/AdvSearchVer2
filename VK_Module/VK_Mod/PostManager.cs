using Newtonsoft.Json.Linq;

namespace VK_Module.VK_Mod
{
    public class PostManager
    {
        public bool HasWordByKey(string word, int key, JObject json)
        {
            string text = json["response"]["items"][key]["text"].ToString();
            if (text.Contains(word))
            {
                return true;
            }
            return false;
        }
    }
}
