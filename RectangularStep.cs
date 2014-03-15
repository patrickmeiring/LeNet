using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedOCR
{
    abstract class RectangularStep : Step
    {
        public RectangularStep(int width, int height, IList<RectangularStep> upstream)
            : base(width * height, upstream == null ? null : upstream.ToList<Step>())
        {
            if (width <= 0 || height <= 0) throw new ArgumentException();
            this.Width = width;
            this.Height = height;
            if (upstream != null)
                this.Upstream = new ReadOnlyCollection<RectangularStep>(upstream);
            else
                this.Upstream = null;
        }


        protected static int WidthOf(IList<RectangularStep> upstream)
        {
            if (upstream == null || upstream.Count == 0) throw new ArgumentException();

            int width = upstream[0].Width;

            if (!upstream.All(step => step.Width == width)) throw new ArgumentException();

            return width;
        }

        protected static int HeightOf(IList<RectangularStep> upstream)
        {
            if (upstream == null || upstream.Count == 0) throw new ArgumentException();

            int height = upstream[0].Height;

            if (!upstream.All(step => step.Height == height)) throw new ArgumentException();

            return height;
        }
        public readonly int Width;
        public readonly int Height;
        public new readonly ReadOnlyCollection<RectangularStep> Upstream;

    }
}
