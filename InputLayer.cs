using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedOCR
{
    class InputStep : RectangularStep
    {
        public InputStep(int width, int height) : base(width, height, null)
        {

        }

        public void SetInputs(double[] inputs)
        {
            Array.Copy(inputs, Output, Length);
        }

        public override Weights Weights
        {
            get { return null; }
        }
    }
}