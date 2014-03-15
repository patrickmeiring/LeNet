namespace AdvancedOCR
{
    partial class LeNetObservationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.inputPicture = new System.Windows.Forms.PictureBox();
            this.outputPicture = new System.Windows.Forms.PictureBox();
            this.firstConvolutionsContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.firstSubsamplingContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.secondConvolutionsContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.secondSubsamplingContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.consolidationPicture = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.inputPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.outputPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.consolidationPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // inputPicture
            // 
            this.inputPicture.Location = new System.Drawing.Point(12, 12);
            this.inputPicture.Name = "inputPicture";
            this.inputPicture.Size = new System.Drawing.Size(96, 96);
            this.inputPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.inputPicture.TabIndex = 0;
            this.inputPicture.TabStop = false;
            // 
            // outputPicture
            // 
            this.outputPicture.Location = new System.Drawing.Point(837, 12);
            this.outputPicture.Name = "outputPicture";
            this.outputPicture.Size = new System.Drawing.Size(42, 84);
            this.outputPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.outputPicture.TabIndex = 1;
            this.outputPicture.TabStop = false;
            // 
            // firstConvolutionsContainer
            // 
            this.firstConvolutionsContainer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.firstConvolutionsContainer.Location = new System.Drawing.Point(131, 12);
            this.firstConvolutionsContainer.Name = "firstConvolutionsContainer";
            this.firstConvolutionsContainer.Size = new System.Drawing.Size(105, 777);
            this.firstConvolutionsContainer.TabIndex = 2;
            // 
            // firstSubsamplingContainer
            // 
            this.firstSubsamplingContainer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.firstSubsamplingContainer.Location = new System.Drawing.Point(242, 12);
            this.firstSubsamplingContainer.Name = "firstSubsamplingContainer";
            this.firstSubsamplingContainer.Size = new System.Drawing.Size(105, 777);
            this.firstSubsamplingContainer.TabIndex = 3;
            // 
            // secondConvolutionsContainer
            // 
            this.secondConvolutionsContainer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.secondConvolutionsContainer.Location = new System.Drawing.Point(353, 12);
            this.secondConvolutionsContainer.Name = "secondConvolutionsContainer";
            this.secondConvolutionsContainer.Size = new System.Drawing.Size(214, 777);
            this.secondConvolutionsContainer.TabIndex = 4;
            // 
            // secondSubsamplingContainer
            // 
            this.secondSubsamplingContainer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.secondSubsamplingContainer.Location = new System.Drawing.Point(573, 12);
            this.secondSubsamplingContainer.Name = "secondSubsamplingContainer";
            this.secondSubsamplingContainer.Size = new System.Drawing.Size(214, 777);
            this.secondSubsamplingContainer.TabIndex = 5;
            // 
            // consolidationPicture
            // 
            this.consolidationPicture.Location = new System.Drawing.Point(802, 12);
            this.consolidationPicture.Name = "consolidationPicture";
            this.consolidationPicture.Size = new System.Drawing.Size(20, 720);
            this.consolidationPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.consolidationPicture.TabIndex = 6;
            this.consolidationPicture.TabStop = false;
            // 
            // LeNetObservationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(899, 797);
            this.Controls.Add(this.consolidationPicture);
            this.Controls.Add(this.secondSubsamplingContainer);
            this.Controls.Add(this.secondConvolutionsContainer);
            this.Controls.Add(this.firstSubsamplingContainer);
            this.Controls.Add(this.firstConvolutionsContainer);
            this.Controls.Add(this.outputPicture);
            this.Controls.Add(this.inputPicture);
            this.Name = "LeNetObservationForm";
            this.ShowIcon = false;
            this.Text = "LeNet Overview";
            ((System.ComponentModel.ISupportInitialize)(this.inputPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.outputPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.consolidationPicture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox inputPicture;
        private System.Windows.Forms.PictureBox outputPicture;
        private System.Windows.Forms.FlowLayoutPanel firstConvolutionsContainer;
        private System.Windows.Forms.FlowLayoutPanel firstSubsamplingContainer;
        private System.Windows.Forms.FlowLayoutPanel secondConvolutionsContainer;
        private System.Windows.Forms.FlowLayoutPanel secondSubsamplingContainer;
        private System.Windows.Forms.PictureBox consolidationPicture;
    }
}

