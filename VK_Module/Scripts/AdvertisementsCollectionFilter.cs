using Services;
using System.Collections.Generic;
using System.Linq;
using VK_Module.Scripts;

namespace Scripts
{
    public class AdvertisementsCollectionFilter
    {
        private AdvertisementsCollectionFilterMode mode;
        public AdvertisementsCollectionFilter(AdvertisementsCollectionFilterMode mode)
        {
            this.mode = mode;
        }

        public List<Advertisement> FilterAdvertisements()
        {
            HomepageService homepageService = new HomepageService();

            List<Advertisement> advertisements = homepageService.GetAllAdvertisements();            

            if (!string.IsNullOrEmpty(mode.HouseType))
            {
                advertisements = advertisements.Where(a => a.HousingType == mode.HouseType).ToList();
            }

            if (!string.IsNullOrEmpty(mode.RoomsCount))
            {
                advertisements = advertisements.Where(a => a.RoomCount == mode.RoomsCount).ToList();
            }

            if (mode.startDate != default && mode.endDate != default)
            {
                advertisements = advertisements.Where(a => a.Date >= mode.startDate && a.Date <= mode.endDate).ToList();
            }

            else if (mode.startDate != default)
            {
                advertisements = advertisements.Where(a => a.Date == mode.startDate).ToList();
            }

            else if (mode.endDate != default)
            {
                advertisements = advertisements.Where(a => a.Date <= mode.endDate).ToList();
            }

            return advertisements;
        }
    }
}
