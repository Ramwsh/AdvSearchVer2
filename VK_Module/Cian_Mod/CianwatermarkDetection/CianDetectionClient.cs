using CianModel;
using Microsoft.ML.Data;
using System.Collections.Generic;
using System.Linq;

namespace VK_Module.Cian_Mod.CianwatermarkDetection
{
    public class CianDetectionClient
    {
        private MLImage image;

        private CianDetectionModel.ModelInput input;
        private CianDetectionModel.ModelOutput output;

        public CianDetectionClient SetInput()
        {
            input = new CianDetectionModel.ModelInput() { Image = image };
            return this;
        }

        public CianDetectionClient SetOutput()
        {
            output = CianDetectionModel.Predict(input);
            return this;
        }

        public CianDetectionClient SetImage(string imagePath)
        {
            image = MLImage.CreateFromFile(imagePath);            
            return this;
        }

        public List<ObjectPredictionData> GetPredictionData()
        {
            if (output != null)
            {
                List<ObjectPredictionData> predictionDataList = new List<ObjectPredictionData>();
                for (int i = 0; i < output.Score.Length; i++)
                {
                    var data = new ObjectPredictionData()
                    {
                        Score = output.Score[i],
                        Label = output.PredictedLabel[i],
                        XTop = output.PredictedBoundingBoxes[i * 4],
                        YTop = output.PredictedBoundingBoxes[i * 4 + 1],
                        XBottom = output.PredictedBoundingBoxes[i * 4 + 2],
                        YBottom = output.PredictedBoundingBoxes[i * 4 + 3],
                    };
                    predictionDataList.Add(data);
                }                
                predictionDataList = predictionDataList.Where(data => data.Score >= 0.2).ToList();
                return predictionDataList;
            }
            return null;
        }
    }
}
