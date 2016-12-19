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
            this.Input.Size = new System.Drawing.Size(284, 220);
            this.Input.TabIndex = 0;
            // 
            // Evaluate
            // 
            this.Evaluate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Evaluate.Location = new System.Drawing.Point(0, 219);
            this.Evaluate.Name = "Evaluate";
            this.Evaluate.Size = new System.Drawing.Size(284, 20);
            this.Evaluate.TabIndex = 1;
            this.Evaluate.Text = "Evaluate";
            this.Evaluate.UseVisualStyleBackColor = true;
            this.Evaluate.Click += new System.EventHandler(this.Evaluate_Click);
            // 
            // Output
            // 
            this.Output.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Output.Location = new System.Drawing.Point(0, 240);
            this.Output.Name = "Output";
            this.Output.Size = new System.Drawing.Size(284, 20);
            this.Output.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
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
    }
}

