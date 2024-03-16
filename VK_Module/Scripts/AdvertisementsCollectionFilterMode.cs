using System;
using VK_Module.MVVM.View;
using VK_Module.Scripts;

namespace Scripts;

public class AdvertisementsCollectionFilterMode
{    
    public string HouseType { get; private set; }
    public string RoomsCount { get; private set; }
    public DateTime startDate { get; private set; }
    public DateTime endDate { get; private set; }    

    public AdvertisementsCollectionFilterMode SetDates(string filterInput)
    {
        try
        {
            if (!string.IsNullOrEmpty(filterInput))
            {
                DateManager dateManager = new DateManager();
                if (filterInput.Contains("-"))
                {
                    var dates = filterInput.Split("-");
                    startDate = dateManager.GetFormattedDateFromHomepage(dates[0])[0];
                    endDate = dateManager.GetFormattedDateFromHomepage(dates[1])[1];
                }
                else
                {
                    startDate = dateManager.GetFormattedDateFromHomepage(filterInput)[0];
                }
            }
        }
        catch
        {
            ErrorView view = new ErrorView();
            view.ErrorText.Text = "Неверный формат даты (дд.мм.уууу)";
            view.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            view.Show();
        }
        return this;
    }

    public AdvertisementsCollectionFilterMode SetHouseType(string filterInput)
    {
        if (!string.IsNullOrEmpty(filterInput))
        {
            HouseType = filterInput;
        }        
        return this;
    }

    public AdvertisementsCollectionFilterMode SetRoomsCount(string filterInput)
    {        
        if (!string.IsNullOrEmpty(filterInput))
        {
            RoomsCount = filterInput;
        }
        return this;
    }    
}
