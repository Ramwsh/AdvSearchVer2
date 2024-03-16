using System.IO;
using System.Linq;
using Microsoft.ML.Trainers.FastTree;
using Microsoft.ML;

namespace HTClassifier
{
    public partial class HousingTypeClassifier
    {
        public const string RetrainFilePath =  @"";
        public const char RetrainSeparatorChar = ';';
        public const bool RetrainHasHeader =  true;

        public static void Train(string outputModelPath, string inputDataFilePath = RetrainFilePath, char separatorChar = RetrainSeparatorChar, bool hasHeader = RetrainHasHeader)
        {
            var mlContext = new MLContext();

            var data = LoadIDataViewFromFile(mlContext, inputDataFilePath, separatorChar, hasHeader);
            var model = RetrainModel(mlContext, data);
            SaveModel(mlContext, model, data, outputModelPath);
        }

        public static IDataView LoadIDataViewFromFile(MLContext mlContext, string inputDataFilePath, char separatorChar, bool hasHeader)
        {
            return mlContext.Data.LoadFromTextFile<ModelInput>(inputDataFilePath, separatorChar, hasHeader);
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
            var pipeline = mlContext.Transforms.Text.FeaturizeText(inputColumnName:@"text",outputColumnName:@"text")      
                                    .Append(mlContext.Transforms.Concatenate(@"Features", new []{@"text"}))      
                                    .Append(mlContext.Transforms.Conversion.MapValueToKey(outputColumnName:@"label",inputColumnName:@"label",addKeyValueAnnotationsAsText:false))      
                                    .Append(mlContext.MulticlassClassification.Trainers.OneVersusAll(binaryEstimator:mlContext.BinaryClassification.Trainers.FastTree(new FastTreeBinaryTrainer.Options(){NumberOfLeaves=4,MinimumExampleCountPerLeaf=20,NumberOfTrees=4,MaximumBinCountPerFeature=254,FeatureFraction=1,LearningRate=0.1,LabelColumnName=@"label",FeatureColumnName=@"Features",DiskTranspose=false}),labelColumnName: @"label"))      
                                    .Append(mlContext.Transforms.Conversion.MapKeyToValue(outputColumnName:@"PredictedLabel",inputColumnName:@"PredictedLabel"));

            return pipeline;
        }
    }
 }
