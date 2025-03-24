using System;
using System.Windows.Forms;

namespace Ex02.View
{
    public partial class GameSettings : Form
    {
        public GameSettings()
        {
            InitializeComponent();
        }
        public int SelectedBoardSize { get; private set; }
        public string SelectedPlayerOneName { get; private set; }
        public string SelectedPlayerTwoName { get; private set; }

        private void playerTwoCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (playerTwoCheckBox.Checked)
            {
                playerTwoName.Enabled = true;
                playerTwoName.Text = string.Empty;
            }
            else
            {
                playerTwoName.Enabled = false;
                playerTwoName.Visible = false;
                playerTwoName.Text = "[Computer]";
            }
        }

        private void confirmButton_Click(object sender, EventArgs e)
        {
            int boardSize = 0;
            bool isValidPlayerTwoName = true;

            if (boardSizeRadioButtons["6X6"].Checked)
            {
                boardSize = 6;
            }
            else if (boardSizeRadioButtons["8X8"].Checked)
            {
                boardSize = 8;
            }
            else if (boardSizeRadioButtons["10X10"].Checked)
            {
                boardSize = 10;
            }

            
            bool isValidPlayerOneName = playerOneName.isValidName();

            if (playerTwoCheckBox.Checked)
            {
                isValidPlayerTwoName = playerTwoName.isValidName();
            }

            if (boardSize == 0)
            {
                DialogResult errorBoardSize = MessageBox.Show(
                    "Please select a board size.",
                    "Error",
                    MessageBoxButtons.OK);
            }

            if (isValidPlayerOneName && isValidPlayerTwoName && boardSize != 0)
            {
                SelectedBoardSize = boardSize;
                SelectedPlayerOneName = playerOneName.Text;
                SelectedPlayerTwoName = playerTwoName.Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
