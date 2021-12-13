using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;

namespace AttendanceStudent.Commons.FaceRecognizer
{
    public interface IRecognizerEngine
    {
        public Task<bool> TrainRecognizer();
        public List<Guid> RecognizeMultipleFaces(Image<Bgr, byte> inputImage);
        public Rectangle[] Detection(Image<Bgr, byte> inputImage);
    }
}