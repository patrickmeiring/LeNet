using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedOCR
{
    abstract class RectangularWeights : BiasedWeights
    {
        public readonly int Width;
        public readonly int Height;
        public readonly int MapCount;

        public RectangularWeights(int width, int height, int maps)
            : base(width * height * maps)
        {
            this.Width = width;
            this.Height = height;
            this.MapCount = maps;
        }


        protected override void PropogateForwardCore(Step downstream)
        {
            RectangularStep step = (RectangularStep)downstream;
            Debug.Assert(MapCount == downstream.Upstream.Count);

            for (int i = 0; i < MapCount; i++)
            {
                PropogateForward(step, i);
            }
        }

        protected abstract void PropogateForward(RectangularStep step, int mapNumber);

        protected override void PreTrainCore(Step downstream)
        {
            RectangularStep step = (RectangularStep)downstream;
            Debug.Assert(MapCount == downstream.Upstream.Count);

            for (int i = 0; i < MapCount; i++)
            {
                PropogateUnitSecondDerivatives(step, i);
            }
            EstimateBiasSecondDerivative(step);
        }

        protected abstract void PropogateUnitSecondDerivatives(RectangularStep downstream, int mapNumber);
        protected abstract void EstimateBiasSecondDerivative(RectangularStep downstream);

        protected override void TrainCore(Step downstream)
        {
            RectangularStep step = (RectangularStep)downstream;
            Debug.Assert(MapCount == downstream.Upstream.Count);

            for (int i = 0; i < MapCount; i++)
            {
                PropogateError(step, i);
            }
        }

        protected abstract void PropogateError(RectangularStep downstream, int mapNumber);
    }
}
