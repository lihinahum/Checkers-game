using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Ex02.View
{
    partial class GameSettings
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            InitializeBoardSizeRadioButtons();
            InitializePlayerLabels();
            InitializeButtons();
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 428);
            this.Name = "GameSettings";
            this.Text = "Game Settings";
            this.MaximizeBox=false;
            this.MinimizeBox=false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition=FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void InitializeBoardSizeRadioButtons()
        {
            boardSizeLabel = new System.Windows.Forms.Label()
            {
                Text = "Board Size:",
                Location = new System.Drawing.Point(18, 14),
                AutoSize = true
            };
            this.Controls.Add(boardSizeLabel);

            string[] boardSizes = { "6X6", "8X8", "10X10" };
            boardSizeRadioButtons = new Dictionary<string, RadioButton>();
            int[] xPositions = { 18, 212, 404 };

            for (int i = 0; i < boardSizes.Length; i++)
            {
                RadioButton radioButton = new RadioButton()
                {
                    Text = boardSizes[i],
                    Location = new System.Drawing.Point(xPositions[i], 69),
                    Size = new System.Drawing.Size(81, 29),
                    AutoSize = true
                };
                boardSizeRadioButtons[boardSizes[i]] = radioButton;

                if (boardSizes[i] == "6X6")
                {
                    boardSizeRadioButtons[boardSizes[i]].Checked = true;
                }

                this.Controls.Add(radioButton);
                
            }
        }

        private void InitializePlayerLabels()
        {
            playersLabel = new System.Windows.Forms.Label()
            {
                Text = "Players:",
                Location = new System.Drawing.Point(18, 145),
                AutoSize = true
            };
            this.Controls.Add(playersLabel);

            playerOneLabel = new System.Windows.Forms.Label()
            {
                Text = "Player 1:",
                Location = new System.Drawing.Point(46, 202),
                AutoSize = true
            };
            this.Controls.Add(playerOneLabel);

            playerTwoLabel = new System.Windows.Forms.Label()
            {
                Text = "Player 2:",
                Location = new System.Drawing.Point(70, 267),
                AutoSize = true
            };
            this.Controls.Add(playerTwoLabel);
        }

        private void InitializeButtons()
        {
            playerTwoCheckBox = new System.Windows.Forms.CheckBox()
            {
                Location = new System.Drawing.Point(34, 269),
                AutoSize = true
            };
            playerTwoCheckBox.CheckedChanged += new EventHandler(this.playerTwoCheckBox_CheckedChanged);
            this.Controls.Add(playerTwoCheckBox);

            playerOneName = new PlayerNameAndError()
            {
                Location = new System.Drawing.Point(164, 190),
                Size = new System.Drawing.Size(449, 70),
                Visible = false
            };
            this.Controls.Add(playerOneName);

            playerTwoName = new PlayerNameAndError()
            {
                Location = new System.Drawing.Point(164, 254),
                Size = new System.Drawing.Size(423, 70),
                Enabled = false,
                Text = "[Computer]",
                Visible = false
            };
            this.Controls.Add(playerTwoName);

            confirmButton = new System.Windows.Forms.Button()
            {
                Text = "Done",
                Location = new System.Drawing.Point(406, 383),
                Size = new System.Drawing.Size(126, 36)
            };
            confirmButton.Click += new EventHandler(this.confirmButton_Click);
            this.Controls.Add(confirmButton);
        }

        private Label boardSizeLabel;
        private Label playersLabel;
        private Label playerOneLabel;
        private Label playerTwoLabel;
        private Button confirmButton;
        private CheckBox playerTwoCheckBox;
        private PlayerNameAndError playerOneName;
        private PlayerNameAndError playerTwoName;
        private Dictionary<string, RadioButton> boardSizeRadioButtons;
    }
}