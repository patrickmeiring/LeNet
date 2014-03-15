using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedOCR
{
    abstract class BiasedWeights : Weights
    {
        public BiasedWeights(int size) : base(size)
        {
            Bias = 0.0;
            BiasStepSize = 0.0;
        }

        protected double Bias;
        protected double BiasStepSize;

        protected override void StartPreTrainingCore()
        {
            BiasStepSize = 0.0;
        }

        public override void PreTrain(Step downstream)
        {
            FinaliseErrorSecondDerivatives(downstream);
            base.PreTrain(downstream);
        }

        public override void Train(Step downstream)
        {
            FinaliseErrorFirstDerivatives(downstream);
            base.Train(downstream);
        }

        protected void FinaliseErrorFirstDerivatives(Step downstream)
        {
            // Calculating the dEj/dWij and dEi/dOi both requires a multiplication by the derivative of the activation function,
            // it is done here once so it doesn't need to be done for each individual calculations.

            // This turns dEk/dOk into dEk/dAk by multiplying it by dOk/dAk
            for (int i = 0; i < downstream.ErrorDerivative.Length; i++)
            {
                double weightedInputs = downstream.WeightedInputs[i];
                double activationDerivative = downstream.CalculateActivationDerivative(weightedInputs);
                downstream.ErrorDerivative[i] *= activationDerivative;
            }
        }

        protected void FinaliseErrorSecondDerivatives(Step downstream)
        {
            for (int i = 0; i < downstream.ErrorDerivative.Length; i++)
            {
                double weightedInputs = downstream.WeightedInputs[i];
                double activationDerivative = downstream.CalculateActivationDerivative(weightedInputs);
                downstream.ErrorDerivative[i] *= activationDerivative * activationDerivative;
            }
        }

        public override void ProprogateForward(Step downstream)
        {
            base.ProprogateForward(downstream);
            FinaliseOutputs(downstream);
        }

        protected void FinaliseOutputs(Step downstream)
        {
            for (int i = 0; i < downstream.WeightedInputs.Length; i++)
            {
                downstream.Output[i] = downstream.CalculateActivation(downstream.WeightedInputs[i]);
            }
        }
    }
}
