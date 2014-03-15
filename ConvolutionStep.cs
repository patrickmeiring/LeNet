using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedOCR
{
    class ConvolutionStep : RectangularStep
    {
        public ConvolutionStep(RectangularStep upstream, int convolutionSize)
            : this(new [] {upstream}, convolutionSize, convolutionSize)
        {
        }

        public ConvolutionStep(IList<RectangularStep> upstream, int convolutionSize)
            : this(upstream, convolutionSize, convolutionSize)
        {
        }


        public ConvolutionStep(RectangularStep upstream, int convolutionWidth, int convolutionHeight)
            : this(new[] { upstream }, convolutionWidth, convolutionHeight)
        {
        }

        public ConvolutionStep(IList<RectangularStep> upstream, int convolutionWidth, int convolutionHeight) : 
            base(WidthOf(upstream) - convolutionWidth + 1,
                 HeightOf(upstream) - convolutionHeight + 1, upstream)
        {
            this.weights = new ConvolutionWeights(convolutionWidth, convolutionHeight, upstream.Count);
        }

        private readonly ConvolutionWeights weights;
        public override Weights Weights
        {
            get { return weights; }
        }

    }
}
