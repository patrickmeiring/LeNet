using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdvancedOCR
{
    partial class LeNetObservationForm : Form
    {
        PictureBox[] firstConvolutions;
        PictureBox[] firstSubsampling;
        PictureBox[] secondConvolutions;
        PictureBox[] secondSubsampling;

        public LeNetObservationForm(LeNetSnapshot snapshot)
        {
            InitializeComponent();
            this.Snapshot = snapshot;

            firstConvolutions = CreateHorizontalPictureBoxes(28*3, snapshot.FirstConvolutions.Length);
            firstConvolutionsContainer.Controls.AddRange(firstConvolutions);
            firstSubsampling = CreateHorizontalPictureBoxes(28 * 3, snapshot.FirstSubsampling.Length);
            firstSubsamplingContainer.Controls.AddRange(firstSubsampling);

            secondConvolutions = CreateHorizontalPictureBoxes(28 * 3, snapshot.SecondConvolutions.Length);
            secondConvolutionsContainer.Controls.AddRange(secondConvolutions);
            secondSubsampling = CreateHorizontalPictureBoxes(28 * 3, snapshot.SecondSubsampling.Length);
            secondSubsamplingContainer.Controls.AddRange(secondSubsampling);

            inputPicture.Paint += inputPicture_Paint;
            Snapshot.Updated += Snapshot_Updated;
            Snapshot.RequestUpdate();
        }

        const int pictureBoxSpacing = 5;


        private PictureBox[] CreateHorizontalPictureBoxes(int width, int number)
        {
            Size size = new Size(width + pictureBoxSpacing * 2, width + pictureBoxSpacing * 2);
            PictureBox[] result = new PictureBox[number];
            for (int i = 0; i < number; i++)
            {
                PictureBox box = new PictureBox();
                box.Size = size;
                box.SizeMode = PictureBoxSizeMode.StretchImage;
                box.Padding = new Padding(pictureBoxSpacing);
                result[i] = box;
            }
            return result;
        }

        void Snapshot_Updated(object sender, EventArgs e)
        {
            if (IsHandleCreated && !Disposing)
            {
                this.BeginInvoke(new Action(SnapshotUpdated));
            }
        }

        private void SnapshotUpdated()
        {
            this.SuspendLayout();
            for (int i = 0; i < firstConvolutions.Length; i++)
                firstConvolutions[i].Image = Snapshot.FirstConvolutions[i].OutputBitmap;
            for (int i = 0; i < firstSubsampling.Length; i++)
                firstSubsampling[i].Image = Snapshot.FirstSubsampling[i].OutputBitmap;
            for (int i = 0; i < secondConvolutions.Length; i++)
                secondConvolutions[i].Image = Snapshot.SecondConvolutions[i].OutputBitmap;
            for (int i = 0; i < secondSubsampling.Length; i++)
                secondSubsampling[i].Image = Snapshot.SecondSubsampling[i].OutputBitmap;
            consolidationPicture.Image = Snapshot.Consolidation.OutputBitmap;
            outputPicture.Image = Snapshot.Output.OutputBitmap;
            inputPicture.Image = Snapshot.Input.OutputBitmap;
            this.ResumeLayout();

        }

        void inputPicture_Paint(object sender, PaintEventArgs e)
        {
            Snapshot.RequestUpdate();
        }

        public readonly LeNetSnapshot Snapshot;


    }
}
