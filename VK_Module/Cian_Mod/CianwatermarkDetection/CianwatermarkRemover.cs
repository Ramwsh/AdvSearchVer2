using System.Drawing;
using System.Collections.Generic;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.XPhoto;
using System.IO;

namespace VK_Module.Cian_Mod.CianwatermarkDetection
{
    public class CianwatermarkRemover
    {        
        private List<ObjectPredictionData> _data;        
        private string _imageFilePath;        

        public CianwatermarkRemover(string imageFilePath, List<ObjectPredictionData> data)
        {
            _data = data;
            _imageFilePath = imageFilePath;
        }

        private List<Rectangle> GetRectangles()
        {
            List<Rectangle> rects = new List<Rectangle>();

            foreach (ObjectPredictionData data in _data)
            {
                Rectangle roi = new Rectangle((int)data.XTop,
                                              (int)data.YTop,
                                              (int)(data.XBottom - data.XTop),
                                              (int)(data.YBottom - data.YTop));
                
                rects.Add(roi);
            }

            return rects;
        }

        //using FSR 

        public void DrawRois()
        {
            List<Rectangle> rois = GetRectangles();

            Mat image = CvInvoke.Imread(_imageFilePath);
            using (Mat imageCopy = image.Clone())
            {
                foreach (var roi in rois)
                {
                    CvInvoke.Rectangle(imageCopy, roi, new MCvScalar(0, 0, 255), 2);
                }

                CvInvoke.Imshow("test", imageCopy);
                //CvInvoke.Imwrite(_imageFilePath, imageCopy);
            }
        }

        public void RemoveLogo()
        {        
            List<Rectangle> rois = GetRectangles();
            Mat image = CvInvoke.Imread(_imageFilePath);
            File.Delete(_imageFilePath);
            Mat mask = new Mat(image.Size, DepthType.Cv8U, 1);
            mask.SetTo(new MCvScalar(255));

            foreach (var roi in rois)
            {
                CvInvoke.Rectangle(mask, roi, new MCvScalar(0), -1);
            }
            XPhotoInvoke.Inpaint(image, mask, image, XPhotoInvoke.InpaintType.FsrBest);            
            
            image.Save(_imageFilePath);
        }
    }
}
