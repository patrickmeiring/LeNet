using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedOCR
{
    abstract class Step
    {
        protected Step(int length, IList<Step> upstream)
            : this(length, upstream, false)
        {

        }

        protected Step(int length, IList<Step> upstream, bool isFinalLayer)
        {
            this.Length = length;
            this.WeightedInputs = new double[length];
            this.Output = new double[length];
            this.ErrorDerivative = new double[length];
            if (upstream != null)
                this.Upstream = new ReadOnlyCollection<Step>(upstream);
            else
                this.Upstream = null;
            this.IsFinalLayer = isFinalLayer;
            
            ClearState();
            ClearError();
        }

        public readonly bool IsFinalLayer;
        public readonly int Length;
        public abstract Weights Weights { get; }
        public ReadOnlyCollection<Step> Upstream { get; private set; }


        internal double[] WeightedInputs;
        internal double[] Output;

        /// <summary>
        /// Represents either the first error derivative if training, or second error derivative of pre-training.
        /// </summary>
        internal double[] ErrorDerivative;

        protected void ClearState()
        {
            Array.Clear(WeightedInputs, 0, Length);
            Array.Clear(Output, 0, Length);
        }

        protected void ClearError()
        {
            Array.Clear(ErrorDerivative, 0, Length);
        }


        public void PropogateForward()
        {
            ClearState();
            Weights.ProprogateForward(this);
        }

        const double X_STRETCH = 2.0 / 3.0;
        const double Y_STRETCH = 1.7159;
        const double DERIVATIVE_STRETCH = 4.57573;

        public virtual double CalculateActivation(double weightedInputs)
        {
            double result = Y_STRETCH * Math.Tanh(X_STRETCH * weightedInputs);
            if (double.IsNaN(result))
                throw new Exception("NaN!");
            return result;
        }

        public virtual double CalculateActivationDerivative(double weightedInputs)
        {
            double coshx = Math.Cosh(X_STRETCH * weightedInputs);
            double denominator = Math.Cosh(2.0 * X_STRETCH * weightedInputs) + 1;
            double result = DERIVATIVE_STRETCH * coshx * coshx / (denominator * denominator);
            if (double.IsNaN(result))
                throw new Exception("NaN!");
            return result;
        }
        
        public void PropogateBackwards()
        {
            if (IsPreTraining)
            {
                Weights.PreTrain(this);
            }
            else
            {
                Weights.Train(this);
            }
            ClearError();
        }

        private bool isPreTraining;
        public bool IsPreTraining
        {
            get { return isPreTraining; }
            set
            {
                if (isPreTraining == value) return;
                isPreTraining = value;
                if (isPreTraining)
                    Weights.StartPreTraining();
                else
                    Weights.CompletePreTraining();
            }
        }


        protected static int SizeOf(IList<Step> upstream)
        {
            if (upstream == null || upstream.Count == 0) throw new ArgumentException();

            int length = upstream[0].Length;

            if (!upstream.All(step => step.Length == length)) throw new ArgumentException();

            return length;
        }


        public override string ToString()
        {
            double MAO = Output.Average(o => Math.Abs(o));
            double MSO = Output.Average(o => o * o);
            double MAI = WeightedInputs.Average(i => Math.Abs(i));
            double MSI = WeightedInputs.Average(i => i * i);
            return string.Format("MAO:{0:0.00000} MSO:{1:0.00000} MAI:{2:0.00000} MSI:{3:0.00000}", MAO, MSO, MAI, MSI);
        }

    }
}
