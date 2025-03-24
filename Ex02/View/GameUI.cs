using System.Collections.Generic;
using System.Windows.Forms;
using Ex02.Model.enums;
using Ex02.View;

namespace Ex02
{
    public class GameUI
    {
        private readonly Dictionary<eErrorTypes, string> r_ErrorMessages = new Dictionary<eErrorTypes, string>()
        {
            { eErrorTypes.MandatoryCaptureRequired, "Normal move is not allowed. There is a mandatory capture move available." },
            { eErrorTypes.CannotMoveBackwards, "Invalid move. You cannot move backwards unless you are a king." },
            { eErrorTypes.IsNotYourPiece,"Invalid move. This is not your piece. Try again." },
            { eErrorTypes.OnlyKingCanEatBackwards,"Invalid move. Only king can eat backwards"},
            { eErrorTypes.InvalidMove, "Invalid move. Try again." },
            { eErrorTypes.InvalidFurtherCapture,"Invalid move. Further capture available from the last position."}
        };

        public GameSettings GameSettings { get; set; }
        public GameBoard GameBoard { get; set; }

        public void DisplayErrorMessage(eErrorTypes i_ErrorType)
        {
            if (r_ErrorMessages.ContainsKey(i_ErrorType))
            {
                MessageBox.Show(r_ErrorMessages[i_ErrorType], "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

