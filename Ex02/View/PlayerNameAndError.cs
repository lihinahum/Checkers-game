using System.Windows.Forms;
using Ex02.Model;

namespace Ex02.View
{
    public partial class PlayerNameAndError : UserControl
    {
        public PlayerNameAndError()
        {
            InitializeComponent();
        }

        public bool Visible
        {
            get { return playerErrorLable.Visible; }
            set { playerErrorLable.Visible = value; }
        }

        public string Text
        {
            get { return playerNameTextBox.Text; }
            set { playerNameTextBox.Text = value; }
        }
      
        public bool isValidName()
        {
            bool isValid = false;

            if (!GameLogic.IsValidNameCheck(this.playerNameTextBox.Text))
            {
                this.Visible = true;
            }
            else
            {
                this.Visible = false;
                isValid = true;
            }

            return isValid;
        }

        public bool Enabled
        {
            get { return playerNameTextBox.Enabled; }
            set { playerNameTextBox.Enabled = value; }
        }
    }
}
