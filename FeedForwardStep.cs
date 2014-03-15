using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedOCR
{
    class FeedForwardStep : Step
    {
        public FeedForwardStep(int outputs, params Step[] upstream)
            : this(outputs, (IList<Step>)upstream)
        {

        }

        public FeedForwardStep(int outputs, IList<Step> upstream)
            : base(outputs, upstream)
        {
            weights = new FeedForwardWeights(SizeOf(upstream) * upstream.Count, outputs);
        }

        private readonly FeedForwardWeights weights;

        public override Weights Weights
        {
            get { return weights; }
        }
    }
}
