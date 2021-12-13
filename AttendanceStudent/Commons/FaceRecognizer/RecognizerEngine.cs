using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AttendanceStudent.Commons.Interfaces;
using AttendanceStudent.Database.Configurations;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace AttendanceStudent.Commons.FaceRecognizer
{
    public class RecognizerEngine : IRecognizerEngine
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ResourceConfiguration _resourceConfiguration;

        private List<Image<Gray, byte>> TrainedFaces;
        private List<int> PersonsLabes;
        private List<Guid> StudentIds;
        private EigenFaceRecognizer _recognizer;

        public RecognizerEngine(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, IOptions<ResourceConfiguration> resourceConfiguration)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _resourceConfiguration = resourceConfiguration.Value;
            TrainedFaces = new List<Image<Gray, byte>>();
            PersonsLabes = new List<int>();
            StudentIds = new List<Guid>();
        }

        public async Task<bool> TrainRecognizer()
        {
            var imagesCount = 0;
            double threshold = 6000;
            TrainedFaces.Clear();
            PersonsLabes.Clear();
            StudentIds.Clear();
            var allFaces = await _unitOfWork.Files.GetAllAsync();
            var studentImages = allFaces.ToList();
            foreach (var item in studentImages)
            {
                var path = Path.Combine(_resourceConfiguration.UploadFolderPath, item.Name);
                var trainedImage = new Image<Gray, byte>(path).Resize(200, 200, Inter.Cubic);
                CvInvoke.EqualizeHist(trainedImage, trainedImage);
                TrainedFaces.Add(trainedImage);
                PersonsLabes.Add(imagesCount);
                StudentIds.Add(item.StudentId);
                imagesCount++;
            }

            if (!TrainedFaces.Any()) return true;
            _recognizer = new EigenFaceRecognizer(imagesCount, threshold);
            Train(TrainedFaces.ToArray(), PersonsLabes.ToArray());
            return true;
        }
        

        public List<Guid> RecognizeMultipleFaces(Image<Bgr, byte> inputImage)
        {
            var resultListStudentIds = new List<Guid>();
            Rectangle[] rectangleFace = Detection(inputImage);
            if (rectangleFace.Length <= 0)
                return resultListStudentIds;

            foreach (var face in rectangleFace)
            {
                inputImage.ROI = face;
                Image<Gray, byte> grayFrame = ToGrayEqualizeFrame(inputImage);
                var result = _recognizer.Predict(grayFrame);
                if (result.Label != -1)
                    resultListStudentIds.Add(StudentIds[result.Label]);
            }

            return resultListStudentIds;
        }

        public Rectangle[] Detection(Image<Bgr, byte> inputImage)
        {
            Image<Gray, byte> grayFrame = inputImage.Convert<Gray, byte>();
            grayFrame._EqualizeHist();
            var faceCascadeClassifierPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Commons/FaceRecognizer", "haarcascade_frontalface_alt.xml");
            var haarCascadeXml = new CascadeClassifier(faceCascadeClassifierPath);
            Rectangle[] rectangleFace = haarCascadeXml.DetectMultiScale(grayFrame, 1.1, 3, Size.Empty, Size.Empty);
            return rectangleFace;
        }

        private static Image<Gray, byte> ToGrayEqualizeFrame(Image<Bgr, byte> inputImage)
        {
            Image<Gray, byte> grayFrame = inputImage.Convert<Gray, byte>().Resize(200,200,Inter.Cubic);
            CvInvoke.EqualizeHist(grayFrame, grayFrame);
            return grayFrame;
        }

        public void Train<TColor, TDepth>(Image<TColor, TDepth>[] images, int[] labels) where TColor : struct, IColor where TDepth : new()
        {
            using (VectorOfMat imgVec = new())
            using (VectorOfInt labelVec = new(labels))
            {
                imgVec.Push(images);
                _recognizer.Train(imgVec, labelVec);
            }
        }
    }
}