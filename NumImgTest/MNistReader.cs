using Neural_Network_Library.Backpropagation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumImgTest
{
    public static class MNISTReader
    {
        private const string TrainImages = "mnist/train-images.idx3-ubyte";
        private const string TrainLabels = "mnist/train-labels.idx1-ubyte";
        private const string TestImages = "mnist/t10k-images.idx3-ubyte";
        private const string TestLabels = "mnist/t10k-labels.idx1-ubyte";

        private static Datapoint[] Read(string imagesPath, string labelsPath)
        {
            BinaryReader labels = new BinaryReader(new FileStream(labelsPath, FileMode.Open));
            BinaryReader images = new BinaryReader(new FileStream(imagesPath, FileMode.Open));

            int magicNumber = images.ReadBigInt32();
            int numberOfImages = images.ReadBigInt32();
            int width = images.ReadBigInt32();
            int height = images.ReadBigInt32();
            int magicLabel = labels.ReadBigInt32();
            int numberOfLabels = labels.ReadBigInt32();

            var td = new Datapoint[numberOfImages];
            for (int i = 0; i < numberOfImages; i++)
            {
                var input = new float[height * width];
                var output = new float[10];
                int label = labels.ReadByte();
                output[label] = 1;
                for (int j = 0; j < input.Length; j++)
                {
                    byte pixel = images.ReadByte();
                    input[j] = pixel / 255f;
                }
                td[i] = new Datapoint(output, input);
            }
            images.BaseStream.Close();
            images.Close();
            labels.BaseStream.Close();
            labels.Close();
            return td;
        }
        public static Datapoint[] TrainingData()
        {
            return Read(TrainImages, TrainLabels);
        }
        public static Datapoint[] TestData()
        {
            return Read(TestImages, TestLabels);
        }
    }

    public static class Extensions
    {
        public static int ReadBigInt32(this BinaryReader br)
        {
            var bytes = br.ReadBytes(sizeof(Int32));
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        public static void ForEach<T>(this T[,] source, Action<int, int> action)
        {
            for (int w = 0; w < source.GetLength(0); w++)
            {
                for (int h = 0; h < source.GetLength(1); h++)
                {
                    action(w, h);
                }
            }
        }
    }
}
