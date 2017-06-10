namespace InnoTecheLearning.WinForms
{
    partial class Form1
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
            this.Input = new System.Windows.Forms.TextBox();
            this.Evaluate = new System.Windows.Forms.Button();
            this.Output = new System.Windows.Forms.TextBox();
            this.Request = new System.Windows.Forms.Button();
            this.Resampler = new System.Windows.Forms.Button();
            this.Hmmm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Input
            // 
            this.Input.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Input.Location = new System.Drawing.Point(0, 1);
            this.Input.Multiline = true;
            this.Input.Name = "Input";
            this.Input.Size = new System.Drawing.Size(483, 335);
            this.Input.TabIndex = 0;
            // 
            // Evaluate
            // 
            this.Evaluate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Evaluate.Location = new System.Drawing.Point(0, 334);
            this.Evaluate.Name = "Evaluate";
            this.Evaluate.Size = new System.Drawing.Size(322, 20);
            this.Evaluate.TabIndex = 1;
            this.Evaluate.Text = "Evaluate";
            this.Evaluate.UseVisualStyleBackColor = true;
            this.Evaluate.Click += new System.EventHandler(this.Evaluate_Click);
            // 
            // Output
            // 
            this.Output.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Output.Location = new System.Drawing.Point(0, 355);
            this.Output.Name = "Output";
            this.Output.Size = new System.Drawing.Size(483, 20);
            this.Output.TabIndex = 2;
            // 
            // Request
            // 
            this.Request.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Request.Location = new System.Drawing.Point(388, 334);
            this.Request.Name = "Request";
            this.Request.Size = new System.Drawing.Size(95, 23);
            this.Request.TabIndex = 3;
            this.Request.Text = "Post dat request";
            this.Request.UseVisualStyleBackColor = true;
            this.Request.Click += new System.EventHandler(this.Request_Click);
            // 
            // Resampler
            // 
            this.Resampler.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Resampler.Location = new System.Drawing.Point(319, 334);
            this.Resampler.Name = "Resampler";
            this.Resampler.Size = new System.Drawing.Size(75, 23);
            this.Resampler.TabIndex = 4;
            this.Resampler.Text = "Resample";
            this.Resampler.UseVisualStyleBackColor = true;
            this.Resampler.Click += new System.EventHandler(this.Resampler_Click);
            // 
            // Hmmm
            // 
            this.Hmmm.Location = new System.Drawing.Point(408, 355);
            this.Hmmm.Name = "Hmmm";
            this.Hmmm.Size = new System.Drawing.Size(75, 23);
            this.Hmmm.TabIndex = 5;
            this.Hmmm.Text = "Press Me";
            this.Hmmm.UseVisualStyleBackColor = true;
            this.Hmmm.Click += new System.EventHandler(this.Hmmm_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 376);
            this.Controls.Add(this.Hmmm);
            this.Controls.Add(this.Resampler);
            this.Controls.Add(this.Request);
            this.Controls.Add(this.Output);
            this.Controls.Add(this.Evaluate);
            this.Controls.Add(this.Input);
            this.Name = "Form1";
            this.Text = "Calculator (Free Mode)";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Input;
        private System.Windows.Forms.Button Evaluate;
        private System.Windows.Forms.TextBox Output;
        private System.Windows.Forms.Button Request;
        private System.Windows.Forms.Button Resampler;
        private System.Windows.Forms.Button Hmmm;
    }
}

