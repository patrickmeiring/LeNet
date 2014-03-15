using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedOCR
{
    class LeNetConfiguration
    {
        private LeNetConfiguration(int classCount)
        {
            ClassCount = classCount;
            Characters = new char[classCount];
            Definitions = new double[classCount * LeNetNetwork.OutputFeedForwardNeurons];
        }

        public const int OutputWidth = 7;
        public const int OutputHeight = 12;

        public readonly int ClassCount;
        public readonly double[] Definitions;
        public readonly char[] Characters;

        public static LeNetConfiguration FromCharacters(params char[] characters)
        {
            LeNetConfiguration result = new LeNetConfiguration(characters.Length);
            Array.Copy(characters, result.Characters, characters.Length);
            for (int i = 0; i < characters.Length; i++)
            {
                char character = characters[i];
                if (characters.Count(item => character == item) > 1) throw new ArgumentException();
                double[] definition = GenerateCharacterDefinition(character);
                Array.Copy(definition, 0, result.Definitions, i * LeNetNetwork.OutputFeedForwardNeurons, definition.Length);
            }
            return result;
        }

        private static readonly Font characterFont = new Font("Lucida Console", 14.0f, FontStyle.Bold, GraphicsUnit.Pixel);

        private static double[] GenerateCharacterDefinition(char input)
        {
            string text = new string(new[] { input });
            Bitmap image = new Bitmap(OutputWidth, OutputHeight);
            Graphics graphics = Graphics.FromImage(image);
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            graphics.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, image.Width, image.Height));
            if (text == "2")
            {
                graphics.DrawString(text, characterFont, new SolidBrush(Color.Black), -2, 0);
            }
            else
            {
                graphics.DrawString(text, characterFont, new SolidBrush(Color.Black), -3, 0);
            }
            graphics.Flush();
            graphics.Dispose();
            return image.ToDoubles();
        }
    }
}
