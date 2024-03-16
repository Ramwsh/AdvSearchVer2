using System;
using System.Collections.Generic;

namespace VK_Module.OK_Mod
{
    class Topic
    {
        public string Url { get; set; }
        public string Id { get; set; }
        public string TopicCreatorName { get; set; }
        public string Text { get; set; }
        public List<string> ImageUrls { get; set; } = new List<string>();        

        public DateTime Date { get; set; }

        public Topic(string text, List<string> imageUrls, string stringDate, string _Id, string _topicCreatorName, string _Url)
        {
            Text = text;
            ImageUrls = imageUrls;
            try
            {
                Date = new DateTime(
                new OKDateYearBuilder().BuildYear(stringDate),
                new OKDateMonthBuilder().BuildMonth(stringDate),
                new OKDateDayBuilder().BuildDay(stringDate)
                );
            }
            catch
            {
                Date = DateTime.Now;
            }            
            Id = _Id;
            TopicCreatorName = _topicCreatorName;
            Url = _Url;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return Text == ((Topic)obj).Text;
        }

        public override int GetHashCode()
        {
            return Text.GetHashCode();
        }
    }
}
