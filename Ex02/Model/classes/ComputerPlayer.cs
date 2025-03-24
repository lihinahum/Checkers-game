namespace Ex02.Model
{
    public class ComputerPlayer : Player
    {
        public ComputerPlayer(string i_Name, ePlayerType i_PlayerType, int i_AmountOfPieces, char i_Symbol)
            : base("[Computer]", ePlayerType.Computer,
                i_AmountOfPieces, 'O')
        {
        }
    }
}
