using System;
using OpenCvSharp;

namespace Day029_03_openCV기본
{
    class Program
    {
        static void Main(string[] args)
        {
            Mat inCvImage, outCvImage, inParaImage; //배열 2개를 준비
            inCvImage = Cv2.ImRead("C:\\images\\Etc_JPG(rectangle)\\airplane.jpg");
           
            outCvImage = new Mat();
            // (1) 그레이 스케일
            //Cv2.CvtColor(inCvImage, outCvImage, ColorConversionCodes.BGR2GRAY);

            // (2) 이진화
            //Cv2.CvtColor(inCvImage, outCvImage, ColorConversionCodes.BGR2GRAY);
            //Cv2.Threshold(outCvImage, outCvImage, 127, 255, ThresholdTypes.Otsu);


            // (14) 색상 검출
            //inCvImage = Cv2.ImRead("C:\\images\\opencv\\tomato.jpg");
            //Mat hsv = new Mat(new OpenCvSharp.Size(inCvImage.Width, inCvImage.Height), MatType.CV_8UC3);
            //Cv2.CvtColor(inCvImage, hsv, ColorConversionCodes.BGR2HSV);
            //Mat[] HSV = Cv2.Split(hsv);
            //Mat H = new Mat(inCvImage.Size(), MatType.CV_8UC1);
            //Cv2.InRange(HSV[0], new Scalar(8), new Scalar(20), H);
            //Cv2.BitwiseAnd(hsv, hsv, outCvImage, H);
            //Cv2.CvtColor(outCvImage, outCvImage, ColorConversionCodes.HSV2BGR);

            inCvImage = Cv2.ImRead("C:\\images\\opencv\\tomato.jpg");
            Cv2.MedianBlur(inCvImage, outCvImage, 5);

            int height = inCvImage.Height;
            int width = inCvImage.Width;
            Mat mask= new Mat(new OpenCvSharp.Size(width, height), MatType.CV_8UC3);
            
            int counter = 0;
            
            CircleSegment[] circles = Cv2.HoughCircles(inCvImage, HoughModes.Gradient, 1, 100, 100, 35, 0, 0);

            for (int i = 0; i < circles.Length; i++)
            {
                //원의 중심점 지정
                OpenCvSharp.Point center = new OpenCvSharp.Point(circles[i].Center.X, circles[i].Center.Y);

                Cv2.Circle(outCvImage, center, (int)circles[i].Radius, Scalar.White, 3);
                Cv2.BitwiseAnd(outCvImage, outCvImage, mask);

                
            }


            // 화면 출력
            Cv2.ImShow("InputImage", inCvImage);
            Cv2.ImShow("outputImage", outCvImage);
            Cv2.WaitKey(0);
        }
    }
}
