namespace VK_Module.OK_Mod
{
    class OKPoster
    {
        private string name;
        private string postUrl;

        public OKPoster(string _parsedName, string _postUrl)
        {
            name = _parsedName;
            postUrl = _postUrl;
        }

        public string GetOKPosterName()
        {
            return name;
        }

        public string GetOKPostURL()
        {
            return postUrl;
        }
    }
}
