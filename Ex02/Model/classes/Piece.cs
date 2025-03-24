using Ex02.Model;

namespace Ex02
{
    public class Piece
    {
        public Position Position;

        public ePieceType Type { get; set; }

        public Player Owner { get; }

        public bool IsKing { get; set; }
     
        public int ID { get; set; }

        public Piece(Player i_Owner, int i_Row, int i_Col, int i_ID)
        {
            Type = ePieceType.Normal;
            Owner = i_Owner;
            IsKing = false;
            Position.CurrentRow = i_Row;
            Position.CurrentCol = i_Col;
            ID = i_ID;
        }

        public void PromoteToKing()
        {
            this.Type = ePieceType.King;
            this.IsKing = true;
        }

        public void SetPosition(Position i_Position)
        {
            Position = i_Position;
        }

        public Position GetPosition()
        {
            return Position;
        }
    }
}
