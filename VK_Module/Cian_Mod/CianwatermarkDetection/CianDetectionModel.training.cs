using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ML.Data;
using Microsoft.ML.TorchSharp;
using Microsoft.ML.Transforms.Image;
using Microsoft.ML;
using Microsoft.ML.TorchSharp.AutoFormerV2;
using System.Text.Json.Nodes;
using System.Text.Json;

namespace CianModel
{
    public partial class CianDetectionModel
    {
        public const string RetrainFilePath = @"C:\Users\rexbur\Desktop\SamplesCore\vott-json-export\CianwatermarkDetectionProject-export.json";
        public const int TrainingImageWidth = 1920;
        public const int TrainingImageHeight = 1080;
        
        public static void Train(string outputModelPath, string inputDataFilePath = RetrainFilePath)
        {
           var mlContext = new MLContext();
           mlContext.GpuDeviceId = 0;
           mlContext.FallbackToCpu = false;
           var data = LoadIDataViewFromVOTTFile(mlContext, inputDataFilePath);
           var model = RetrainModel(mlContext, data);
           SaveModel(mlContext, model, data, outputModelPath);
        }
        
        public static IDataView LoadIDataViewFromVOTTFile(MLContext mlContext, string inputDataFilePath)
        {
           return mlContext.Data.LoadFromEnumerable(LoadFromVott(inputDataFilePath));
        }

        private static IEnumerable<ModelInput> LoadFromVott(string inputDataFilePath)
        {
            JsonNode jsonNode;
            using (StreamReader r = new StreamReader(inputDataFilePath))
            {
                string json = r.ReadToEnd();
                jsonNode = JsonSerializer.Deserialize<JsonNode>(json);
            }

            var imageData = new List<ModelInput>();
            foreach (KeyValuePair<string, JsonNode> asset in jsonNode["assets"].AsObject())
            {
                var labelList = new List<string>();
                var boxList = new List<float>();

                var sourceWidth = asset.Value["asset"]["size"]["width"].GetValue<float>();
                var sourceHeight = asset.Value["asset"]["size"]["height"].GetValue<float>();   

                CalculateAspectAndOffset(sourceWidth, sourceHeight, TrainingImageWidth, TrainingImageHeight, out float xOffset, out float yOffset, out float aspect);

                foreach (var region in asset.Value["regions"].AsArray())
                {
                    foreach (var tag in region["tags"].AsArray())
                    {
                        labelList.Add(tag.GetValue<string>());
                        var boundingBox = region["boundingBox"];
                        var left = boundingBox["left"].GetValue<float>();
                        var top = boundingBox["top"].GetValue<float>();
                        var width = boundingBox["width"].GetValue<float>();
                        var height = boundingBox["height"].GetValue<float>();

                        boxList.Add(xOffset + (left * aspect));
                        boxList.Add(yOffset + (top * aspect));
                        boxList.Add(xOffset + ((left + width) * aspect));
                        boxList.Add(yOffset + ((top + height) * aspect));
                    }
                    
                }    

                var mlImage = MLImage.CreateFromFile(asset.Value["asset"]["path"].GetValue<string>().Replace("file:", ""));
                var modelInput = new ModelInput()
                {
                    Image = mlImage,
                    Labels = labelList.ToArray(),
                    Box = boxList.ToArray(),
                };    

                imageData.Add(modelInput);
            }

            return imageData;
        }

        private static void CalculateAspectAndOffset(float sourceWidth, float sourceHeight, float destinationWidth, float destinationHeight, out float xOffset, out float yOffset, out float aspect)
        {
            float widthAspect = destinationWidth / sourceWidth;
            float heightAspect = destinationHeight / sourceHeight;
            xOffset = 0;
            yOffset = 0;
            if (heightAspect < widthAspect)
            {
                aspect = heightAspect;
                xOffset = (destinationWidth - (sourceWidth * aspect)) / 2;     
            }    
            else
            {
                aspect = widthAspect;
                yOffset = (destinationHeight - (sourceHeight * aspect)) / 2;
            }
        }

        public static void SaveModel(MLContext mlContext, ITransformer model, IDataView data, string modelSavePath)
        {            
            DataViewSchema dataViewSchema = data.Schema;

            using (var fs = File.Create(modelSavePath))
            {
                mlContext.Model.Save(model, dataViewSchema, fs);
            }
        }

        public static ITransformer RetrainModel(MLContext mlContext, IDataView trainData)
        {
            var pipeline = BuildPipeline(mlContext);
            var model = pipeline.Fit(trainData);

            return model;
        }
        
        public static IEstimator<ITransformer> BuildPipeline(MLContext mlContext)
        {         
            var pipeline = mlContext.Transforms.Conversion.MapValueToKey(outputColumnName:@"Labels",inputColumnName:@"Labels",addKeyValueAnnotationsAsText:false)      
                                    .Append(mlContext.Transforms.ResizeImages(outputColumnName:@"Image",inputColumnName:@"Image",imageHeight:TrainingImageHeight,imageWidth:TrainingImageWidth,cropAnchor:ImageResizingEstimator.Anchor.Center,resizing:ImageResizingEstimator.ResizingKind.IsoPad))      
                                    .Append(mlContext.MulticlassClassification.Trainers.ObjectDetection(new ObjectDetectionTrainer.Options(){LabelColumnName=@"Labels",PredictedLabelColumnName=@"PredictedLabel",BoundingBoxColumnName=@"Box",ImageColumnName=@"Image",ScoreColumnName=@"score",MaxEpoch=10,InitLearningRate=1,WeightDecay=0,}))      
                                    .Append(mlContext.Transforms.Conversion.MapKeyToValue(outputColumnName:@"PredictedLabel",inputColumnName:@"PredictedLabel"));

            return pipeline;
        }
    }
 }
