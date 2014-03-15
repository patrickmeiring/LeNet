using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedOCR
{
    struct DataSetItem
    {
        public double[] Inputs;
        public char Character;

        private static DataSetItem[] LoadLeCunSet(string imagePath, string labelPath)
        {
            FileStream ImageStream = new FileStream(imagePath, FileMode.Open);
            FileStream LabelStream = new FileStream(labelPath, FileMode.Open);
            BinaryReader brImage = new BinaryReader(ImageStream);
            BinaryReader brLabel = new BinaryReader(LabelStream);

            if (ReadBigEndianInteger(brImage) != 2051)
                throw new InvalidDataException("Invalid magic in specified image file.");
            if (ReadBigEndianInteger(brLabel) != 2049)
                throw new InvalidDataException("Invalid magic in specified label file.");

            int ItemCount = ReadBigEndianInteger(brImage);
            if (ReadBigEndianInteger(brLabel) != ItemCount)
                throw new InvalidDataException("Number of images and labels do not match.");

            int rows = ReadBigEndianInteger(brImage);
            int columns = ReadBigEndianInteger(brImage);

            DataSetItem[] items = new DataSetItem[ItemCount];
            for (int i = 0; i < ItemCount; i++)
            {
                char character = (char)(brLabel.ReadByte().ToString()[0]);

                // Read image with border of 2 on all sides.
                double[] inputs = new double[(rows + 4) * (columns + 4)];
                for (int x = 0; x < (rows + 4); x++)
                    for (int y = 0; y < (columns + 4); y++)
                    {
                        inputs[x + y * (rows + 4)] = -0.1;
                    }

                int inputIndex = 2;
                for (int y = 2; y < (columns + 2); y++)
                {
                    for (int x = 2; x < (rows + 2); x++)
                    {

                        // Background pixel is -0.1 (black) and Foreground is 1.175. 
                        // Refer to page 7 of LeCun's document on Gradient-based learning applied to document recognition.
                        inputs[inputIndex] = (((double)brImage.ReadByte()) / 255.0) * 1.275 - 0.1;

                        inputIndex += 1;
                    }
                    inputIndex += 4;
                }
                items[i] = new DataSetItem() { Inputs = inputs, Character = character };
            }
            return items;
        }

        private static void SaveNativeSet(string filePath, DataSetItem[] set)
        {
            FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
            BinaryWriter bw = new BinaryWriter(fs);

            bw.Write(set.Length);
            bw.Write(set[0].Inputs.Length);

            for (int i = 0; i < set.Length; i++)
            {
                bw.Write(set[i].Character);
                for (int j = 0; j < set[i].Inputs.Length; j++)
                {
                    double value = Math.Round(((set[i].Inputs[j] + 0.1) / 1.275) * 255.0);
                    byte byteValue = (byte)value;
                    bw.Write(byteValue);
                }
            }

            bw.Flush();
            fs.SetLength(fs.Position);
            fs.Close();
        }

        private static DataSetItem[] LoadNativeSet(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryReader br = new BinaryReader(fs);

            int count = br.ReadInt32();
            int inputCount = br.ReadInt32();
            DataSetItem[] result = new DataSetItem[count];

            for (int i = 0; i < result.Length; i++)
            {
                char character = br.ReadChar();
                double[] inputs = new double[inputCount];
                for (int j = 0; j < inputCount; j++)
                {
                    byte b = br.ReadByte();
                    inputs[j] = (((double)b) / 255.0) * 1.275 - 0.1;
                }


                DataSetItem item = new DataSetItem();
                item.Character = character;
                item.Inputs = inputs;
                result[i] =item;
            }
            br.Close();
            return result;
        }

        
        private static int ReadBigEndianInteger(BinaryReader br)
        {
            byte[] data = br.ReadBytes(4);
            byte[] result = new byte[] { data[3], data[2], data[1], data[0] };
            return BitConverter.ToInt32(result, 0);
        }


        public const string TrainingDatasetImages = @"MNIST\MNIST-train-images.dat";
        public const string TrainingDatasetLabels = @"MNIST\MNIST-train-labels.dat";
        public const string GeneralisationDatasetImages = @"MNIST\MNIST-test-images.dat";
        public const string GeneralisationDatasetLabels = @"MNIST\MNIST-test-labels.dat";

        public const string TrainingDatasetCache = "MNIST-training.dat";
        public const string GeneralisationDatasetCache = "MNIST-generalisation.dat";
        
        private static IList<DataSetItem> trainingDataset;
        private static IList<DataSetItem> generalisationDataset;

        private static IList<DataSetItem> LoadTrainingDataset()
        {
            if (!File.Exists(TrainingDatasetCache))
            {
                DataSetItem[] set = DataSetItem.LoadLeCunSet(TrainingDatasetImages, TrainingDatasetLabels);
                DataSetItem.SaveNativeSet(TrainingDatasetCache, set);
                return set;
            } 
            else
            {
                return DataSetItem.LoadNativeSet(TrainingDatasetCache);
            }
        }

        private static IList<DataSetItem> LoadGeneralisationDataset()
        {
            if (!File.Exists(GeneralisationDatasetCache))
            {
                DataSetItem[] set = DataSetItem.LoadLeCunSet(GeneralisationDatasetImages, GeneralisationDatasetLabels);
                DataSetItem.SaveNativeSet(GeneralisationDatasetCache, set);
                return set;
            }
            else
            {
                return DataSetItem.LoadNativeSet(GeneralisationDatasetCache);
            }
        }

        public static IList<DataSetItem> GetTrainingSet()
        {
            if (trainingDataset == null)
            {
                trainingDataset = LoadTrainingDataset();
            }
            return trainingDataset;
        }

        public static IList<DataSetItem> GetGeneralisationSet()
        {
            if (generalisationDataset == null)
            {
                generalisationDataset = LoadGeneralisationDataset();
            }
            return generalisationDataset;
        }
    }
}
