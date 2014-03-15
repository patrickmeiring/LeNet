using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedOCR
{
    class FeedForwardWeights : BiasedWeights
    {
        public readonly int InputNeurons;
        public readonly int OutputNeurons;

        public FeedForwardWeights(int inputLength, int outputLength) : base(inputLength * outputLength)
        {
            InputNeurons = inputLength;
            OutputNeurons = outputLength;
            Weight = new double[inputLength * outputLength];
            WeightStepSize = new double[inputLength * outputLength];

            Randomise(Weight, InputNeurons);
            Array.Clear(WeightStepSize, 0, Size);
        }


        protected readonly double[] Weight;
        protected readonly double[] WeightStepSize;


        protected override void PropogateForwardCore(Step downstream)
        {
            Debug.Assert(InputNeurons % downstream.Upstream.Count == 0);
            int neuronsPerUpstream = InputNeurons / downstream.Upstream.Count;

            int inputIndex = 0;
            foreach (Step upstream in downstream.Upstream)
            {
                Debug.Assert(inputIndex + upstream.Length <= InputNeurons);

                for (int i = 0; i < neuronsPerUpstream; i++)
                {
                    PropogateForward(downstream, upstream, i, inputIndex++);
                }
            }
        }

        protected void PropogateForward(Step downstream, Step upstream, int upstreamNeuron, int inputNeuron)
        {
            int weightIndex = inputNeuron * OutputNeurons;

            double upstreamNeuronOutput = upstream.Output[upstreamNeuron];
            double weightedSum = Bias;
            for (int o = 0; o < OutputNeurons; o++)
            {
                downstream.WeightedInputs[o] += upstreamNeuronOutput * Weight[weightIndex++];
            }
        }

        protected override void StartPreTrainingCore()
        {
            base.StartPreTrainingCore();
            Array.Clear(WeightStepSize, 0, Size);
        }
        
        protected override void PreTrainCore(Step downstream)
        {
            Debug.Assert(InputNeurons % downstream.Upstream.Count == 0);
            int neuronsPerUpstream = InputNeurons / downstream.Upstream.Count;

            int inputNeuron = 0;
            foreach (Step upstream in downstream.Upstream)
            {
                Debug.Assert(upstream.Length == neuronsPerUpstream);

                for (int i = 0; i < neuronsPerUpstream; i++)
                {
                    PropogateSecondDerivatives(downstream, upstream, i, inputNeuron++);
                }
            }
            EstimateBiasSecondDerivative(downstream);
        }

        protected void PropogateSecondDerivatives(Step downstream, Step upstream, int upstreamNeuron, int inputNeuron)
        {
            int weightIndex = inputNeuron * OutputNeurons;

            double upstreamState = upstream.Output[upstreamNeuron];
            double upstreamErrorSecondDerivative = 0.0;

            for (int output = 0; output < OutputNeurons; output++)
            {
                double downstreamErrorSecondDerivative = downstream.ErrorDerivative[output]; // (d^2)E/(dAj)^2, where Aj is the sum of inputs to this downstream unit.

                // Here we calculate (d^2)Ej/(dWij)^2 by multiplying the 2nd derivative of E with respect to the sum of inputs, Aj
                // by the state of Oi, the upstream unit, squared. Refer to Equation 25 in document.
                // The summing happening here is described by equation 23.
                double weight2ndDerivative = downstreamErrorSecondDerivative * upstreamState * upstreamState;

                WeightStepSize[weightIndex] = weight2ndDerivative;

                double weight = Weight[weightIndex];

                // This is implementing the last sigma of Equation 27.
                // This propogates error second derivatives back to previous layer, but will need to be multiplied by the second derivative
                // of the activation function at the previous layer.
                upstreamErrorSecondDerivative += weight * weight * downstreamErrorSecondDerivative;

                weightIndex += 1;

            }

            upstream.ErrorDerivative[upstreamNeuron] += upstreamErrorSecondDerivative;
        }

        protected void EstimateBiasSecondDerivative(Step downstream)
        {
            for (int i = 0; i < downstream.Length; i++)
            {
                // Calculating the sum of: second derivatives of error with respect to the bias weight.
                // Note that the bias is implemented as an always-on Neuron with a (the same) weight to the outputs neurons.
                BiasStepSize += downstream.ErrorDerivative[i] * 1.0 * 1.0;
            }
        }

        protected override void CompletePreTrainingCore()
        {
            Debug.Assert(PreTrainingSamples > 0);
            double averageHkk = 0;

            double sampleCount = (double)PreTrainingSamples;
            // Divide each of the 2nd derivative sums by the number of samples used to make the estimation, then  convert into step size.
            BiasStepSize = GlobalLearningRate / (GlobalMu + (BiasStepSize / sampleCount));

            for (int i = 0; i < Size; i++)
            {
                double Hkk = WeightStepSize[i] / sampleCount;
                averageHkk += Hkk;
                WeightStepSize[i] = GlobalLearningRate / (GlobalMu + Hkk);
            }

            averageHkk /= (double)(Size);
            System.Diagnostics.Debug.WriteLine("Average Hkk value: " + averageHkk.ToString(), "NNFFInfo");

        }

        protected override void TrainCore(Step downstream)
        {
            Debug.Assert(InputNeurons % downstream.Upstream.Count == 0);
            int neuronsPerUpstream = InputNeurons / downstream.Upstream.Count;

            int inputNeuron = 0;
            foreach (Step upstream in downstream.Upstream)
            {
                Debug.Assert(upstream.Length == neuronsPerUpstream);
                for (int i = 0; i < neuronsPerUpstream; i++)
                {
                    PropogateError(downstream, upstream, i, inputNeuron++);
                }
            }
        }

        protected void PropogateError(Step downstream, Step upstream, int upstreamNeuron, int inputNeuron)
        {
            int weightIndex = inputNeuron * OutputNeurons;

            double upstreamState = upstream.Output[upstreamNeuron];

            double inputError = 0.0;
            for (int output = 0; output < OutputNeurons; output++)
            {
                double downstreamErrorDerivative = downstream.ErrorDerivative[output]; 

                // Calculate inputs error gradient by taking the sum, for all outputs of
                // dEk/dAj multiplied by dAj/dOj (w/sum =dEj/dOj);
                inputError += (downstreamErrorDerivative * Weight[weightIndex]);

                // Calculate the Weight's first derivative with respect to the error
                double weightErrorGradient = downstreamErrorDerivative * upstreamState;
                double deltaWeight = WeightStepSize[weightIndex] * weightErrorGradient;
                Weight[weightIndex] -= deltaWeight;

                weightIndex += 1;
            }
            upstream.ErrorDerivative[upstreamNeuron] = inputError;
        }

        public override string ToString()
        {
            double averageWeight = Weight.Average(w => Math.Abs(w));
            return String.Format("AW:{0:0.00000}", averageWeight);
        }
    }
}
