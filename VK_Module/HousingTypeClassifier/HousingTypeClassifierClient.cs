using System.Linq;

namespace VK_Module.HousingTypeClassifier
{
    public class HousingTypeClassifierClient
    {           
        private HTClassifier.HousingTypeClassifier.ModelInput input;
        private HTClassifier.HousingTypeClassifier.ModelOutput output;

        public HousingTypeClassifierClient SetText(string text)
        {
            input = new HTClassifier.HousingTypeClassifier.ModelInput() { Text = text };
            return this;
        }

        public string Classify()
        {
            output = HTClassifier.HousingTypeClassifier.Predict(input);            
            if (output != null)
            {
                var dictionary = HTClassifier.HousingTypeClassifier.GetSortedScoresWithLabels(output).ToDictionary();

                var maxKeyValuePair = dictionary.FirstOrDefault(x => x.Value == dictionary.Values.Max());                

                if (maxKeyValuePair.Value >= 0.5)
                {
                    return dictionary.Keys.First();
                }                
            }
            return "Не определен";            
        }
    }
}
