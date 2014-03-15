using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedOCR
{
    class SubsamplingStep : RectangularStep
    {
        public SubsamplingStep(RectangularStep upstream, int subsamplingSize)
            : this(new[] { upstream}, subsamplingSize)
        {

        }


        public SubsamplingStep(RectangularStep upstream, int subsamplingWidth, int subsamplingHeight)
            : this(new[] { upstream }, subsamplingWidth, subsamplingHeight)
        {

        }

        public SubsamplingStep(IList<RectangularStep> upstream, int subsamplingSize)
            : this(upstream, subsamplingSize, subsamplingSize)
        {

        }

        public SubsamplingStep(IList<RectangularStep> upstream, int subsamplingWidth, int subsamplingHeight)
             : base(WidthOf(upstream) / subsamplingWidth,
                 HeightOf(upstream) / subsamplingHeight, upstream)
        {
            if (WidthOf(upstream) % subsamplingWidth != 0) throw new ArgumentException();
            if (HeightOf(upstream) % subsamplingHeight != 0) throw new ArgumentException();
            weights = new SubsamplingWeights(subsamplingWidth, subsamplingHeight);
        }


        private readonly SubsamplingWeights weights;
        public override Weights Weights
        {
            get { return weights; }
        }

    }
}
