using System;

namespace VK_Module.Scripts
{
    class AdvertisementBuilder
    {
        private Advertisement advertisement;

        public AdvertisementBuilder()
        {
            advertisement = new Advertisement();
        }

        public AdvertisementBuilder SetId(string id)
        {
            advertisement.Id = id;
            return this;
        }

        public AdvertisementBuilder SetLink(string link)
        {
            advertisement.Link = link;
            return this;
        }

        public AdvertisementBuilder SetDescription(string description)
        {
            advertisement.Description = description;
            return this;
        }

        public AdvertisementBuilder SetPoster(string poster)
        {
            advertisement.Poster = poster;
            return this;
        }

        public AdvertisementBuilder SetDate(DateTime date)
        {
            advertisement.Date = date;
            return this;
        }

        public AdvertisementBuilder SetImageLinks(string imageLinks)
        {
            advertisement.ImageLinks = imageLinks;
            return this;
        }

        public AdvertisementBuilder SetRoomCount(string roomCount)
        {
            advertisement.RoomCount = roomCount;
            return this;
        }

        public AdvertisementBuilder SetHousingType(string housingType)
        {
            advertisement.HousingType = housingType;
            return this;
        }

        public AdvertisementBuilder SetState(string state)
        {
            advertisement.State = state;
            return this;
        }

        public Advertisement Build()
        {
            return advertisement;
        }
    }
}
