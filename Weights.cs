using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AdvancedOCR
{
    abstract class Weights 
    {
        protected Weights(int size)
        {
            Size = size;
        }

        public readonly int Size;

        public static double GlobalMu;
        public static double GlobalLearningRate;

        protected int PreTrainingSamples { get; private set; }
        public void StartPreTraining()
        {
            PreTrainingSamples = 0;
            StartPreTrainingCore();
        }

        protected abstract void StartPreTrainingCore();

        public virtual void PreTrain(Step step)
        {
            PreTrainingSamples += 1;
            PreTrainCore(step);
        }

        protected abstract void PreTrainCore(Step downstream);

        protected abstract void CompletePreTrainingCore();

        public void CompletePreTraining()
        {
            Debug.Assert(PreTrainingSamples != 0);
            CompletePreTrainingCore();
        }

        public virtual void ProprogateForward(Step downstream)
        {
            PropogateForwardCore(downstream);
        }

        protected abstract void PropogateForwardCore(Step step);
        
        public virtual void Train(Step downstream)
        {
            TrainCore(downstream);
        }

        protected abstract void TrainCore(Step downstream);

        protected static Random random = new Random(0);
        protected static void Randomise(double[] weights, int fanIn)
        {
            double fanInDouble = (double)fanIn;
            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = (random.NextDouble() - 0.5) * (4.8 / fanInDouble);
            }
        }

        protected static double RandomWeight(int fanIn)
        {
            return (random.NextDouble() - 0.5) * (4.8 / fanIn);
        }
    }
}
