namespace Ex02.View
{
    partial class PlayerNameAndError
    {
        private System.ComponentModel.IContainer components = null;

        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.playerNameTextBox = new System.Windows.Forms.TextBox();
            this.playerErrorLable = new System.Windows.Forms.Label();
            this.SuspendLayout();
            
            this.playerNameTextBox.Location = new System.Drawing.Point(6, 8);
            this.playerNameTextBox.Name = "playerNameTextBox";
            this.playerNameTextBox.Size = new System.Drawing.Size(156, 31);
            this.playerNameTextBox.TabIndex = 0;
           
            this.playerErrorLable.AutoSize = true;
            this.playerErrorLable.ForeColor = System.Drawing.Color.Red;
            this.playerErrorLable.Location = new System.Drawing.Point(174, 11);
            this.playerErrorLable.Name = "playerErrorLable";
            this.playerErrorLable.Size = new System.Drawing.Size(183, 25);
            this.playerErrorLable.TabIndex = 1;
            this.playerErrorLable.Text = "Name is not valid!";
            this.playerErrorLable.Visible = false;
           
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.playerErrorLable);
            this.Controls.Add(this.playerNameTextBox);
            this.Name = "PlayerNameAndError";
            this.Size = new System.Drawing.Size(358, 46);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox playerNameTextBox;
        private System.Windows.Forms.Label playerErrorLable;
    }
}
