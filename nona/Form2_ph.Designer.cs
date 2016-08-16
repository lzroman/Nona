namespace nona
{
    partial class Form2_ph
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
            this.pb_ph = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pb_ph)).BeginInit();
            this.SuspendLayout();
            // 
            // pb_ph
            // 
            this.pb_ph.Location = new System.Drawing.Point(12, 12);
            this.pb_ph.Name = "pb_ph";
            this.pb_ph.Size = new System.Drawing.Size(500, 500);
            this.pb_ph.TabIndex = 0;
            this.pb_ph.TabStop = false;
            this.pb_ph.Paint += new System.Windows.Forms.PaintEventHandler(this.pb_ph_Paint);
            // 
            // Form2_ph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 522);
            this.ControlBox = false;
            this.Controls.Add(this.pb_ph);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form2_ph";
            this.ShowInTaskbar = false;
            this.Text = "Фазовый портрет";
            ((System.ComponentModel.ISupportInitialize)(this.pb_ph)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pb_ph;
    }
}