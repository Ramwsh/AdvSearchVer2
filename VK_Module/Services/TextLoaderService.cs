using VK_Module.Scripts;

namespace Services
{
    public class TextLoaderService
    {
        private Advertisement advertisement;

        public TextLoaderService(Advertisement advertisement)
        {
            this.advertisement = advertisement;
        }

        public string LoadTextData()
        {
            string text = advertisement.Description;
            text = text.Trim();
            text = text.Replace("  ", " ").Trim();
            text = text.Replace("\n", "");
            text = text.ToLower();
            return text;
        }
    }
}
