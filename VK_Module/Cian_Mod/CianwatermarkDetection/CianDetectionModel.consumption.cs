using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.IO;

namespace CianModel
{
    public partial class CianDetectionModel
    {                
        public class ModelInput
        {
            [LoadColumn(0)]
            [ColumnName(@"Labels")]
            public string[] Labels { get; set; }

            [LoadColumn(1)]
            [ColumnName(@"Image")]
            [Microsoft.ML.Transforms.Image.ImageType(800, 600)]
            public MLImage Image { get; set; }

            [LoadColumn(2)]
            [ColumnName(@"Box")]
            public float[] Box { get; set; }

        }
                
        public class ModelOutput
        {
            [ColumnName(@"Labels")]
            public uint[] Labels { get; set; }

            [ColumnName(@"Image")]
            [Microsoft.ML.Transforms.Image.ImageType(1080, 1920)]            
            public MLImage Image { get; set; }

            [ColumnName(@"Box")]
            public float[] Box { get; set; }

            [ColumnName(@"PredictedLabel")]
            public string[] PredictedLabel { get; set; }

            [ColumnName(@"score")]
            public float[] Score { get; set; }

            [ColumnName(@"PredictedBoundingBoxes")]
            public float[] PredictedBoundingBoxes { get; set; }

        }        

        private static string MLNetModelPath = Path.GetFullPath(Environment.CurrentDirectory + @"\Cian_Mod\CianwatermarkDetection\CianDetectionModel.mlnet");

        public static readonly Lazy<PredictionEngine<ModelInput, ModelOutput>> PredictEngine = new Lazy<PredictionEngine<ModelInput, ModelOutput>>(() => CreatePredictEngine(), true);


        private static PredictionEngine<ModelInput, ModelOutput> CreatePredictEngine()
        {
            var mlContext = new MLContext();
            mlContext.GpuDeviceId = 0;
            mlContext.FallbackToCpu = true;
            ITransformer mlModel = mlContext.Model.Load(MLNetModelPath, out var _);
            return mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
        }
        
        public static ModelOutput Predict(ModelInput input)
        {
            var predEngine = PredictEngine.Value;
            var output = predEngine.Predict(input);

            CalculateAspectAndOffset(input.Image.Width, input.Image.Height, TrainingImageWidth, TrainingImageHeight, out float xOffset, out float yOffset, out float aspect);

            if (output.PredictedBoundingBoxes != null && output.PredictedBoundingBoxes.Length > 0)
            {
                for (int x = 0; x < output.PredictedBoundingBoxes.Length; x += 2)
                {
                    output.PredictedBoundingBoxes[x] = (output.PredictedBoundingBoxes[x] - xOffset) / aspect;
                    output.PredictedBoundingBoxes[x + 1] = (output.PredictedBoundingBoxes[x + 1] - yOffset) / aspect;
                }
            }
            return output;
        }
    }
}
