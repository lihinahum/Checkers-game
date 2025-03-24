using System;
using System.Windows.Forms;

namespace Ex02.View
{
    public partial class PlayerAndScore : UserControl
    {
        public PlayerAndScore()
        {
            this.Load+= PlayerAndScore_Load;
            InitializeComponent();
        }

        private void PlayerAndScore_Load(object sender, EventArgs e)
        {
            FlowLayoutPanel panel = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };

            panel.Controls.Add(playerName);
            panel.Controls.Add(playerScore);
            this.Controls.Add(panel);
            
        }

        public string Text
        {
            get { return playerName.Text; }
            set { playerName.Text = value; }
        }

        public string Score
        {
            get { return playerScore.Text; }
            set { playerScore.Text = value; }
        }
    }
}
