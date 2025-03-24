using System.Collections.Generic;

namespace Ex02
{
    public class Player
    {
        public readonly string r_Name;
        private int m_Points;

        public ePlayerType PlayerType { get; private set; }

        public List<Piece> Pieces { get; set; }
    
        public char PieceShape { get; }

        public string GetPlayerName()
        {
            return r_Name;
        }

        public Player(string i_Name, ePlayerType i_PlayerType, int i_AmountOfPieces, char i_PieceShape)
        {
            r_Name = i_Name;
            PlayerType = i_PlayerType;
            m_Points = 0;
            PieceShape = i_PieceShape;
            Pieces = new List<Piece>(i_AmountOfPieces);
        }

        public void InitializePiecesForPlayer()
        {
            Pieces.Clear();
        }

        public int Points
        {
            get { return m_Points; }
        }

        public void AddPoints()
        {
            m_Points += CalculatePoints();
        }

        public void RemovePiece(int i_ID)
        {
            Pieces.RemoveAll(piece => piece.ID == i_ID);
        }

        public void AddPiece(Piece i_Piece)
        {
            Pieces.Add(i_Piece);
        }

        public int CalculatePoints()
        {
            return 4 * GetKingPiecesCount() + GetRegularPiecesCount();
        }

        public int GetRegularPiecesCount()
        {
            int countRegularPieces = 0;

            foreach (Piece piece in Pieces)
            {
                if (!piece.IsKing)
                {
                    countRegularPieces++;
                }
            }

            return countRegularPieces;
        }

        public int GetKingPiecesCount()
        {
            int countKingPieces = 0;

            foreach (Piece piece in Pieces)
            {
                if (piece.IsKing)
                {
                    countKingPieces++;
                }
            }

            return countKingPieces;
        }
    }
}
