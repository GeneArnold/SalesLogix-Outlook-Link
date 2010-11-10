namespace SLX_Outlook_AddIn
{
    partial class OpportunityDetails
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
            this.txtOppoDescription = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtOppoDescription
            // 
            this.txtOppoDescription.Location = new System.Drawing.Point(0, 0);
            this.txtOppoDescription.Name = "txtOppoDescription";
            this.txtOppoDescription.Size = new System.Drawing.Size(100, 20);
            this.txtOppoDescription.TabIndex = 0;
            // 
            // OpportunityDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.txtOppoDescription);
            this.Name = "OpportunityDetails";
            this.Text = "OpportunityDetails";
            this.Load += new System.EventHandler(this.OpportunityDetails_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtOppoDescription;
    }
}