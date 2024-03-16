namespace VK_Module.Scripts
{
    public class AdvertisementRepository
    {
        public string Tag { get; set; }
        public string Path { get; set; }
        public AdvertisementRepository(string tag, string path)
        {
            Tag = tag;
            Path = path;
        }
    }
}
