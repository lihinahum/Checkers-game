using System.Drawing;

namespace Ex02.View
{
    partial class PlayerAndScore
    {
        private System.ComponentModel.IContainer components = null;
        readonly int r_TextSize = 10;

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
            this.playerName = new System.Windows.Forms.Label();
            this.playerScore = new System.Windows.Forms.Label();
            this.SuspendLayout();
           
            this.playerName.AutoSize = true;
            this.playerName.Location = new System.Drawing.Point(4, 17);
            this.playerName.Name = "playerName";
            this.playerName.Size = new System.Drawing.Size(79, 25);
            this.playerName.Font = new Font("Arial", r_TextSize, FontStyle.Bold);
            this.playerName.TabIndex = 0;
            this.playerName.Text = "Player:";
            
            this.playerScore.AutoSize = true;
            this.playerScore.Name = "playerScore";
            this.playerScore.Top = playerName.Bottom;
            this.playerScore.Size = new System.Drawing.Size(24, 25);
            this.playerScore.Font = new Font("Arial", r_TextSize, FontStyle.Bold);
            this.playerScore.TabIndex = 1;
            this.playerScore.Text = "0";
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.playerScore);
            this.Controls.Add(this.playerName);
            this.Name = "PlayerAndScore";
            this.Size = new System.Drawing.Size(184, 57);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label playerName;
        private System.Windows.Forms.Label playerScore;
    }
}
