using System.Collections.Generic;
using Ex02.Model;

namespace Ex02
{
    public class Board
    {
        private Piece[,] m_Board;

        public int BoardSize { get; set; }

        public void ClearBoard()
        {
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    m_Board[row, col] = null; 
                }
            }
        }

        public Piece GetPiece(int i_Row, int i_Col)
        {
            Piece newPiece = m_Board[i_Row, i_Col];

            return newPiece;
        }

        public Piece GetPiece(Position i_Position)
        {
            return m_Board[i_Position.CurrentRow, i_Position.CurrentCol];
        }

        public Board(int i_Size)
        {
            m_Board = new Piece[i_Size, i_Size];
            BoardSize = i_Size;
        }

        public void InitBoard(Player i_Player1, Player i_Player2)
        {
            int rowsToPlacePieces = (BoardSize / 2) - 1;
            int idCounter = 1;

            for (int row = 0; row < rowsToPlacePieces; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    if ((row + col) % 2 == 1)
                    {
                        m_Board[row, col] = new Piece(i_Player2, row, col, idCounter);
                        i_Player2.AddPiece(m_Board[row, col]);
                        idCounter++;
                    }
                    else
                    {
                        m_Board[row, col] = null;
                    }
                }
            }

            for (int row = BoardSize - rowsToPlacePieces; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    if ((row + col) % 2 == 1)
                    {
                        m_Board[row, col] = new Piece(i_Player1, row, col, idCounter);
                        i_Player1.AddPiece(m_Board[row, col]);
                        idCounter++;
                    }
                    else
                    {
                        m_Board[row, col] = null;
                    }
                }
            }
        }

        public void MovePiece(Position i_FromPosition, Position i_ToPosition,Piece i_piece)
        {
            m_Board[i_ToPosition.CurrentRow, i_ToPosition.CurrentCol] = i_piece;
            i_piece.SetPosition(i_ToPosition);
            m_Board[i_FromPosition.CurrentRow, i_FromPosition.CurrentCol] = null;
        }

        public void RemovePiece(Position i_Position, ref Player io_Player)
        {
            Piece pieceToRemove = m_Board[i_Position.CurrentRow, i_Position.CurrentCol];

            if (pieceToRemove != null)
            {
                io_Player.RemovePiece(pieceToRemove.ID);
                m_Board[i_Position.CurrentRow, i_Position.CurrentCol] = null;
            }
        }

        public List<Piece> GetAllPieces()
        {
            int rows = m_Board.GetLength(0);
            int cols = m_Board.GetLength(1);
            List<Piece> allPieces = new List<Piece>(rows * cols);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (m_Board[i, j] != null)
                    {
                        allPieces.Add(m_Board[i, j]);
                    }
                }
            }

            return allPieces;
        }
    }
}
