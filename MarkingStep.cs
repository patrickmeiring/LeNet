using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedOCR
{
    class MarkingStep : Step
    {
        public MarkingStep(Step upstream, LeNetConfiguration configuration)
            : this(new[] { upstream }, configuration)
        {

        }

        public MarkingStep(IList<Step> upstream, LeNetConfiguration configuration)
            : base(configuration.ClassCount, upstream, true)
        {
            if (SizeOf(upstream) * upstream.Count != LeNetNetwork.OutputFeedForwardNeurons) throw new ArgumentException();
            weights = new MarkingWeights(configuration);
        
        }

        public int CorrectClass
        {
            get { return weights.CorrectClass; }
            set { weights.CorrectClass = value; }
        }

        protected readonly MarkingWeights weights;
        public override Weights Weights
        {
            get { return weights; }
        }

        public override double CalculateActivation(double weightedInputs)
        {
            double coshx = Math.Cosh((2.0 / 3.0) * weightedInputs);
            double denominator = Math.Cosh((4.0 / 3.0) * weightedInputs) + 1;
            return 4.57573 * coshx * coshx / (denominator * denominator);
        }

        public override double CalculateActivationDerivative(double weightedInputs)
        {
            double coshTwoThirdsx = Math.Cosh((2.0 / 3.0) * weightedInputs);
            double sinhFourThirdsx = Math.Sinh((4.0 / 3.0) * weightedInputs);
            double denominator = Math.Cosh((4.0 / 3.0) * weightedInputs) + 1;
            denominator = denominator * denominator * denominator;

            return (-6.10098 * sinhFourThirdsx * coshTwoThirdsx * coshTwoThirdsx) / denominator;
        }

    }
}
