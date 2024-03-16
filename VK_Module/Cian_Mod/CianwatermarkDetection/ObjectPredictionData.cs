namespace VK_Module.Cian_Mod.CianwatermarkDetection
{
    public class ObjectPredictionData
    {
        public float Score { get; set; }

        public string Label { get; set; }

        public float XTop { get; set; }
        public float YTop { get; set; }
        public float XBottom { get; set; }
        public float YBottom { get; set; }
    }
}
