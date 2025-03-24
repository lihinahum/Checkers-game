using System;
using System.Windows.Forms;

namespace Ex02.View
{
    public partial class GameBoard : Form
    {
        public event Action<string> OnMoveSelected;
        public GameBoard(string i_PlayerOneName, string i_PlayerTwoName, Board i_Board, string i_CurrentPlayer,int i_PlayerOneScore, int i_PlayerTwoScore)
        {
            InitializeComponent(i_PlayerOneName, i_PlayerTwoName, i_Board, i_CurrentPlayer,i_PlayerOneScore.ToString(),i_PlayerTwoScore.ToString());
        }

        private void notifyManagerOfMove(int i_FromRow, int i_FromCol, int i_ToRow, int i_ToCol)
        {
            string moveInput = convertToBoardPosition(i_FromRow, i_FromCol) + ">" + convertToBoardPosition(i_ToRow, i_ToCol);
            OnMoveSelected?.Invoke(moveInput); 
        }

        private string convertToBoardPosition(int i_Row, int i_Col)
        {
            char columnLetter = (char)('a' + i_Col);
            char rowLetter = (char)('A' + i_Row);

            return string.Format("{0}{1}", rowLetter, columnLetter);
        }
    }
}
