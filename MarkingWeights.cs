using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedOCR
{
    class MarkingWeights : Weights
    {
        public MarkingWeights(LeNetConfiguration configuration) : base(configuration.ClassCount * LeNetNetwork.OutputFeedForwardNeurons)
        {
            if (configuration.Definitions == null) throw new ArgumentException();
            ClassStateDefinitions = configuration.Definitions;
            InputLength = LeNetNetwork.OutputFeedForwardNeurons;
            ClassCount = configuration.ClassCount;
        }

        protected readonly int InputLength;
        protected readonly int ClassCount;

        public int CorrectClass
        {
            get { return correctClass; }
            set { correctClass = value; }
        }
        protected int correctClass;
        protected readonly double[] ClassStateDefinitions;

        protected override void StartPreTrainingCore()
        {
        }

        protected override void PreTrainCore(Step downstream)
        {
            int inputIndex = 0;
            foreach (Step upstream in downstream.Upstream)
            {
                Debug.Assert(inputIndex + upstream.Length <= InputLength);
                for (int i = 0; i < upstream.Length; i++)
                {
                    // Error second derivative relative to output is constant, as first derivative is 2.0 * (state - desiredState).
                    upstream.ErrorDerivative[inputIndex] = 2.0;

                    inputIndex += 1;
                }
            }
        }


        protected override void CompletePreTrainingCore()
        {
        }

        protected override void PropogateForwardCore(Step downstream)
        {
            Debug.Assert(downstream.Upstream.Count == 1);

            for (int o = 0; o < ClassCount; o++)
            {
                PropogateForward(downstream, o);
            }
        }

        protected void PropogateForward(Step downstream, int output)
        {
            double sumSquaredError = 0;
            int inputIndex = 0;
            int definitionIndex = output * InputLength;
            foreach (Step upstream in downstream.Upstream)
            {
                Debug.Assert(inputIndex + upstream.Length <= InputLength);
                for (int i = 0; i < upstream.Length; i++)
                {
                    double difference = upstream.Output[i] - ClassStateDefinitions[definitionIndex];
                    sumSquaredError += difference * difference;

                    inputIndex += 1;
                    definitionIndex += 1;
                }
            }
            downstream.Output[output] = sumSquaredError;
        }

        protected override void TrainCore(Step downstream)
        {
            int inputIndex = 0;
            int definitionIndex = correctClass * InputLength;
            foreach (Step upstream in downstream.Upstream)
            {
                Debug.Assert(inputIndex + upstream.Length <= InputLength);
                for (int i = 0; i < upstream.Length; i++)
                {
                    double desiredState = ClassStateDefinitions[definitionIndex];

                    double firstDerivative = 2.0 * (upstream.Output[inputIndex] - desiredState);
                    upstream.ErrorDerivative[inputIndex] = firstDerivative;

                    inputIndex += 1;
                    definitionIndex += 1;
                }
            }
        }

    }
}
