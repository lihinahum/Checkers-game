using Ex02.Model.enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ex02.Model
{
    public class GameLogic
    {
        private const int k_MaxLength = 20;
        public event Action<List<Position>,List<Position>> onPotentialMoves;
        public Random m_Random = new Random();

        private Move selectRandomMove(List<Move> i_Moves)
        {
            int randomIndex = m_Random.Next(0, i_Moves.Count);

            return i_Moves[randomIndex];
        }

        public void CheckAvailableMovesForSelectedPiece(Board i_Board, Piece i_SelectedPiece)
        {
            Position currentPosition = i_SelectedPiece.Position;
            List<Position> validMoves = getAllAvailableMovesForPiece(i_Board, i_SelectedPiece);
            List<Position> captureMoves = new List<Position>();

            foreach (Position pos in validMoves)
            {
                if (isCaptureMove(i_Board, i_SelectedPiece, new Move(currentPosition.CurrentRow, currentPosition.CurrentCol,
                        pos.CurrentRow, pos.CurrentCol)))
                {
                    captureMoves.Add(pos);
                }
            }

            onPotentialMoves?.Invoke(validMoves,captureMoves);
        }

        public int GetAmountOfPiecesForPlayer(int i_BoardSize)
        {
            int halfSize = i_BoardSize / 2;

            return halfSize * (halfSize - 1);
        }

        public static bool IsValidNameCheck(string i_PlayerName)
        {
            return !string.IsNullOrEmpty(i_PlayerName) && i_PlayerName.Length <= k_MaxLength &&
                   !i_PlayerName.Any(char.IsWhiteSpace) && IsEnglishLettersOnly(i_PlayerName);
        }

        public static bool IsEnglishLettersOnly(string i_NameInput)
        {
            bool isEnglishLetters = true;

            foreach (char c in i_NameInput)
            {
                if (!char.IsLetter(c) || !(c >= 'A' && c <= 'Z') && !(c >= 'a' && c <= 'z'))
                {
                    isEnglishLetters = false;
                }
            }

            return isEnglishLetters;
        }

        private bool isInsideBoard(Board i_Board, int i_Row, int i_Col)
        {
            return i_Row >= 0 && i_Row < i_Board.BoardSize &&
                   i_Col >= 0 && i_Col < i_Board.BoardSize;
        }

        private bool isMoveWithinBoard(Board i_Board, Move i_Move)
        {
            return isInsideBoard(i_Board, i_Move.FromRow, i_Move.FromCol) &&
                   isInsideBoard(i_Board, i_Move.ToRow, i_Move.ToCol);
        }

        private bool isPositionWithinBoard(Position i_Pos, Board i_Board)
        {
            return isInsideBoard(i_Board, i_Pos.CurrentRow, i_Pos.CurrentCol);
        }

        private bool isSafeMove(Board i_Board, Piece i_Piece, Move i_CurrentMove)
        {
            bool isSafe = true;
            Position originalPosition = i_Piece.Position;
            Position targetPosition = new Position(i_CurrentMove.ToRow, i_CurrentMove.ToCol);

            i_Board.MovePiece(originalPosition, targetPosition, i_Piece);

            foreach (Piece opponentPiece in i_Board.GetAllPieces())
            {
                if (opponentPiece.Owner != i_Piece.Owner)
                {
                    List<Position> opponentPotentialMoves = getAllAvailableMovesForPiece(i_Board, opponentPiece);

                    foreach (Position opponentMove in opponentPotentialMoves)
                    {
                        int jumpedRow = (opponentPiece.Position.CurrentRow + opponentMove.CurrentRow) / 2;
                        int jumpedCol = (opponentPiece.Position.CurrentCol + opponentMove.CurrentCol) / 2;

                        if (isCaptureMove(i_Board, opponentPiece,
                                new Move(opponentPiece.Position.CurrentRow, opponentPiece.Position.CurrentCol,
                                    opponentMove.CurrentRow, opponentMove.CurrentCol, false)) &&
                            jumpedRow == i_CurrentMove.ToRow &&
                            jumpedCol == i_CurrentMove.ToCol)
                        {
                            isSafe = false;
                            break;
                        }
                    }

                    if (!isSafe)
                    {
                        break;
                    }
                }
            }

            i_Board.MovePiece(new Position(i_CurrentMove.ToRow, i_CurrentMove.ToCol), originalPosition, i_Piece);
            return isSafe;
        }

        private List<Position> getAllAvailableMovesForPiece(Board i_Board, Piece i_Piece)
        {
            List<Position> toMoves = new List<Position>();
            Position currentPiecePosition = i_Piece.GetPosition();
            List<Position> potentialMoves = new List<Position>();

            potentialMoves.AddRange(getNormalAndKingMoves(currentPiecePosition, i_Piece.Owner.PlayerType,
                i_Piece.IsKing));

            foreach (Position position in potentialMoves)
            {
                int rowDistance = Math.Abs(position.CurrentRow - currentPiecePosition.CurrentRow);
                int colDistance = Math.Abs(position.CurrentCol - currentPiecePosition.CurrentCol);

                if (isPositionWithinBoard(position, i_Board) && i_Board.GetPiece(position) == null)
                {
                    if (rowDistance == 1 && colDistance == 1 && i_Board.GetPiece(position) == null)
                    {
                        toMoves.Add(position);
                    }
                    else if (rowDistance == 2 && colDistance == 2)
                    {
                        int capturedRow = (currentPiecePosition.CurrentRow + position.CurrentRow) / 2;
                        int capturedCol = (currentPiecePosition.CurrentCol + position.CurrentCol) / 2;
                        Position capturedPosition = new Position(capturedRow, capturedCol);

                        if (isPositionWithinBoard(capturedPosition, i_Board) &&
                            i_Board.GetPiece(capturedPosition) != null &&
                            i_Board.GetPiece(capturedPosition).Owner != i_Piece.Owner &&
                            i_Board.GetPiece(position) == null)
                        {
                            toMoves.Add(position);
                        }
                    }
                }
            }

            return toMoves;
        }

        private List<Position> getNormalAndKingMoves(Position i_CurrentPosition, ePlayerType i_PlayerType,
            bool i_IsKing)
        {
            List<Position> moves = new List<Position>();
            int forwardRowDirection = (i_PlayerType == ePlayerType.Player1) ? -1 : 1;
            int backwardRowDirection = (i_PlayerType == ePlayerType.Player1) ? 1 : -1;

            moves.AddRange(new[]
            {
                new Position(i_CurrentPosition.CurrentRow + forwardRowDirection,
                    i_CurrentPosition.CurrentCol + 1), // forward right
                new Position(i_CurrentPosition.CurrentRow + forwardRowDirection,
                    i_CurrentPosition.CurrentCol - 1), // forward left
                new Position(i_CurrentPosition.CurrentRow + 2 * forwardRowDirection,
                    i_CurrentPosition.CurrentCol + 2), // capture forward right
                new Position(i_CurrentPosition.CurrentRow + 2 * forwardRowDirection,
                    i_CurrentPosition.CurrentCol - 2) // capture forward left
            });

            if (i_IsKing)
            {
                moves.AddRange(new[]
                {
                    new Position(i_CurrentPosition.CurrentRow + backwardRowDirection,
                        i_CurrentPosition.CurrentCol + 1), // backward right
                    new Position(i_CurrentPosition.CurrentRow + backwardRowDirection,
                        i_CurrentPosition.CurrentCol - 1), // backward left
                    new Position(i_CurrentPosition.CurrentRow + 2 * backwardRowDirection,
                        i_CurrentPosition.CurrentCol + 2), // capture backward right
                    new Position(i_CurrentPosition.CurrentRow + 2 * backwardRowDirection,
                        i_CurrentPosition.CurrentCol - 2) // capture backward left
                });
            }

            return moves;
        }

        private bool anyMandatoryCapture(Board i_Board, Player i_Player)
        {
            bool hasCapture = false;

            foreach (Piece piece in i_Player.Pieces)
            {
                List<(Position enemyPos, Position afterEatPos)> capturePositions =
                    getCapturePositions(i_Board, piece.Position, piece.IsKing, i_Player.PlayerType);

                foreach (var (enemyPos, afterEatPos) in capturePositions)
                {
                    Move captureMove = new Move(piece.Position.CurrentRow, piece.Position.CurrentCol,
                        afterEatPos.CurrentRow, afterEatPos.CurrentCol, true);

                    if (isCaptureMove(i_Board, piece, captureMove))
                    {
                        hasCapture = true;
                        break;
                    }
                }

                if (hasCapture)
                {
                    break;
                }
            }


            return hasCapture;
        }

        private List<(Position, Position)> getCapturePositions(Board i_Board, Position i_Position, bool i_IsKing,
            ePlayerType i_PlayerType)
        {
            List<(Position, Position)> capturePositions = new List<(Position, Position)>();
            List<int> rowDirections = new List<int>();

            if (i_IsKing)
            {
                rowDirections.Add(-1);
                rowDirections.Add(1);
            }
            else
            {
                rowDirections.Add(i_PlayerType == ePlayerType.Player1 ? -1 : 1);
            }

            int[] colDirections = new[] { -1, 1 };

            foreach (int rowDir in rowDirections)
            {
                foreach (int colDir in colDirections)
                {
                    Position enemyPos = new Position(i_Position.CurrentRow + rowDir, i_Position.CurrentCol + colDir);
                    Position afterEatPos = new Position(i_Position.CurrentRow + 2 * rowDir,
                        i_Position.CurrentCol + 2 * colDir);
                    Move enemyMove = new Move(i_Position.CurrentRow, i_Position.CurrentCol, enemyPos.CurrentRow,
                        enemyPos.CurrentCol, true);
                    Move afterEatMove = new Move(enemyPos.CurrentRow, enemyPos.CurrentCol, afterEatPos.CurrentRow,
                        afterEatPos.CurrentCol, true);

                    if (isMoveWithinBoard(i_Board, enemyMove) && isMoveWithinBoard(i_Board, afterEatMove))
                    {
                        capturePositions.Add((enemyPos, afterEatPos));
                    }
                }
            }

            return capturePositions;
        }

        private bool isCaptureMove(Board i_Board, Piece i_Piece, Move i_Move)
        {
            bool isCaptureMove = false;
            int rowDifference = i_Move.ToRow - i_Move.FromRow;
            int colDifference = i_Move.ToCol - i_Move.FromCol;

            if (Math.Abs(rowDifference) == 2 && Math.Abs(colDifference) == 2)
            {
                int capturedRow = (i_Move.FromRow + i_Move.ToRow) / 2;
                int capturedCol = (i_Move.FromCol + i_Move.ToCol) / 2;
                Piece capturedPiece = i_Board.GetPiece(new Position(capturedRow, capturedCol));

                if (capturedPiece != null && capturedPiece.Owner != i_Piece.Owner)
                {
                    isCaptureMove = i_Board.GetPiece(new Position(i_Move.ToRow, i_Move.ToCol)) == null;
                }
            }

            return isCaptureMove;
        }

        public List<Move> GetAllPossibleCaptures(Board i_Board, Player i_Player, List<Move> i_ValidMoves)
        {
            List<Move> captureMoves = new List<Move>();

            foreach (Move move in i_ValidMoves)
            {
                Piece piece = i_Board.GetPiece(new Position(move.FromRow, move.FromCol));
                if (piece != null && isCaptureMove(i_Board, piece, move))
                {
                    captureMoves.Add(move);
                }
            }

            return captureMoves;
        }

        public List<Move> GetAllSafeMovesForComputer(Board i_Board, Player i_Player, List<Move> i_ValidMoves)
        {
            List<Move> safeMoves = new List<Move>();

            foreach (Move move in i_ValidMoves)
            {
                Piece piece = i_Board.GetPiece(new Position(move.FromRow, move.FromCol));
                if (piece != null && isSafeMove(i_Board, piece, move))
                {
                    safeMoves.Add(move);
                }
            }

            return safeMoves;
        }

        public bool IsFurtherCapturePossible(Board i_Board, Player i_Player, Move i_LastPlayerMove)
        {
            bool isFurtherCaptureFound = false;
            bool isCapturingMove = false;
            Position i_LastPlayerMovePosition = new Position(i_LastPlayerMove.ToRow, i_LastPlayerMove.ToCol);
            Piece piece = i_Board.GetPiece(i_LastPlayerMovePosition);

            if (piece != null)
            {
                List<Position> potentialMoves = getAllAvailableMovesForPiece(i_Board, piece);

                foreach (Position move in potentialMoves)
                {
                    Move potentialCapture = new Move(i_LastPlayerMovePosition.CurrentRow,
                        i_LastPlayerMovePosition.CurrentCol, move.CurrentRow, move.CurrentCol, isCapturingMove);
                    if (isCaptureMove(i_Board, piece, potentialCapture))
                    {
                        isFurtherCaptureFound = true;
                        break;
                    }
                }
            }

            return isFurtherCaptureFound;
        }

        private void updateGameSettingsAfterValidMove(Board i_Board, Player i_Player, Move i_Move, Piece i_Piece)
        {
            Position fromPosition = new Position(i_Move.FromRow, i_Move.FromCol);
            Position toPosition = new Position(i_Move.ToRow, i_Move.ToCol);
            i_Board.MovePiece(fromPosition, toPosition, i_Piece);

            promoteToKingIfNeeded(i_Board, toPosition, i_Player);
        }

        private void removeCapturedPiece(Board i_Board, Move i_CurrentMove)
        {
            int capturedRow = (i_CurrentMove.FromRow + i_CurrentMove.ToRow) / 2;
            int capturedCol = (i_CurrentMove.FromCol + i_CurrentMove.ToCol) / 2;
            Piece capturedPiece = i_Board.GetPiece(new Position(capturedRow, capturedCol));

            if (capturedPiece != null)
            {
                Player capturedPlayer = capturedPiece.Owner;
                i_Board.RemovePiece(new Position(capturedRow, capturedCol), ref capturedPlayer);
            }
        }

        public void MakeComputerMove(Board i_Board, ComputerPlayer i_ComputerPlayer, out Move o_ComputerMove)
        {
            List<Move> validMoves = GetAllValidMoves(i_Board, i_ComputerPlayer);
            List<Move> captureMoves = GetAllPossibleCaptures(i_Board, i_ComputerPlayer, validMoves);
            List<Move> safeMoves = GetAllSafeMovesForComputer(i_Board, i_ComputerPlayer, validMoves);
            List<Move> safeCaptureMoves = GetAllSafeMovesForComputer(i_Board, i_ComputerPlayer, captureMoves);
            Move selectedMove;
            
            if (safeCaptureMoves.Count > 0)
            {
                selectedMove = selectRandomMove(safeCaptureMoves);
            }
            else if (captureMoves.Count > 0)
            {
                selectedMove = selectRandomMove(captureMoves);
            }
            else if (safeMoves.Count > 0)
            {
                selectedMove = selectRandomMove(safeMoves);
            }
            else
            {
                selectedMove = selectRandomMove(validMoves);
            }

            o_ComputerMove = selectedMove;
            PlayerMove(i_Board, i_ComputerPlayer, selectedMove);
        }
        
        public void PlayerMove(Board i_Board, Player i_CurrentPlayer, Move i_CurrentMove)
        {
            Piece piece = i_Board.GetPiece(new Position(i_CurrentMove.FromRow, i_CurrentMove.FromCol));

            if (isCaptureMove(i_Board, piece, i_CurrentMove))
            {
                i_CurrentMove.IsCaptureMove = true;
                removeCapturedPiece(i_Board, i_CurrentMove);
            }

            updateGameSettingsAfterValidMove(i_Board, i_CurrentPlayer, i_CurrentMove, piece);
        }

        public List<Move> GetAllValidMoves(Board i_Board, Player i_Player)
        {
            List<Move> validMoves = new List<Move>();
            bool isCapturingMove = false;

            foreach (Piece piece in i_Player.Pieces)
            {
                List<Position> toPositions = getAllAvailableMovesForPiece(i_Board, piece);

                foreach (Position position in toPositions)
                {
                    Move move = new Move(piece.Position.CurrentRow, piece.Position.CurrentCol, position.CurrentRow, position.CurrentCol, isCapturingMove);
                    if (IsValidMove(i_Board, i_Player, move))
                    {
                        validMoves.Add(move);
                    }
                }
            }

            return validMoves;
        }

        public bool IsValidMove(Board i_Board, Player i_Player, Move i_Move)
        {
            Piece piece = i_Board.GetPiece(new Position(i_Move.FromRow, i_Move.FromCol));
            bool isValidMove = false;

            if (piece != null && piece.Owner == i_Player && isMoveWithinBoard(i_Board, i_Move))
            {
                List<Position> positions = getAllAvailableMovesForPiece(i_Board, piece);
                Position movePosition = new Position(i_Move.ToRow, i_Move.ToCol);

                foreach (Position position in positions)
                {
                    if (position.CurrentRow == movePosition.CurrentRow &&
                        position.CurrentCol == movePosition.CurrentCol)
                    {
                        bool isMandatoryCapture = anyMandatoryCapture(i_Board, i_Player);
                        bool isCapture = isCaptureMove(i_Board, piece, i_Move);

                        if (!isMandatoryCapture || isCapture)
                        {
                            int rowDistance = Math.Abs(i_Move.ToRow - i_Move.FromRow);
                            int colDistance = Math.Abs(i_Move.ToCol - i_Move.FromCol);

                            if (rowDistance == 1 && colDistance == 1 || isCapture)
                            {
                                isValidMove = true;
                                break; 
                            }
                        }
                    }
                }
            }

            return isValidMove;
        }

        public eGameResults DetermineGameResult(Board i_Board, List<Player> i_Players, eGameMode iEGameMode)
        {
            eGameResults gameResults = eGameResults.Ongoing;
            Player player1 = i_Players[0];
            Player player2 = i_Players[1];
            bool player1HasNoMoves = player1.Pieces.Count == 0 || GetAllValidMoves(i_Board, player1).Count == 0;
            bool player2HasNoMoves = player2.Pieces.Count == 0 || GetAllValidMoves(i_Board, player2).Count == 0;

            if (player1HasNoMoves && player2HasNoMoves)
            {
                gameResults = DetermineTie(i_Board, i_Players, iEGameMode);
            }
            else
            {
                if (player1HasNoMoves)
                {
                    if (iEGameMode == eGameMode.HumanPlayer)
                    {
                        gameResults = eGameResults.Player2Win;
                    }
                    else
                    {
                        gameResults = eGameResults.ComputerWin;
                    }

                    player2.AddPoints();
                }
                else if (player2HasNoMoves)
                {
                    player1.AddPoints();
                    gameResults = eGameResults.Player1Win;
                }
            }

            return gameResults;
        }

        public eGameResults DetermineTie(Board i_Board, List<Player> i_Players, eGameMode iEGameMode)
        {
            eGameResults gameResults = eGameResults.Tie;
            Player player1 = i_Players[0];
            Player player2 = i_Players[1];

            player1.CalculatePoints();
            player2.CalculatePoints();

            if (Math.Abs(player1.Points - player2.Points) > 0)
            {
                gameResults = eGameResults.Player1Win;
            }
            else if (Math.Abs(player2.Points - player1.Points) > 0)
            {
                gameResults = iEGameMode == eGameMode.HumanPlayer ? eGameResults.Player2Win : eGameResults.ComputerWin;
            }
            else
            {
                gameResults = eGameResults.Tie;
            }

            return gameResults;
        }

        private void promoteToKingIfNeeded(Board i_Board, Position i_CurrentPosition, Player i_CurrentPlayer)
        {
            bool isPromotionRow = (i_CurrentPosition.CurrentRow == 0 || i_CurrentPosition.CurrentRow == i_Board.BoardSize - 1);

            if (isPromotionRow)
            {
                Piece pieceToPromote = i_Board.GetPiece(i_CurrentPosition);

                for (int i = 0; i < i_CurrentPlayer.Pieces.Count; i++)
                {
                    if (i_CurrentPlayer.Pieces[i].ID == pieceToPromote.ID)
                    {
                        i_CurrentPlayer.Pieces[i].PromoteToKing();
                        break;
                    }
                }
            }
        }

        public eErrorTypes GetErrorType(Board i_Board, Player i_Player, Move i_Move)
        {
            eErrorTypes errorType = eErrorTypes.None; // Default to no error
            Piece piece = i_Board.GetPiece(i_Move.FromRow, i_Move.FromCol);

            if (piece == null)
            {
                errorType = eErrorTypes.InvalidMove;
            }
            else if (piece.Owner != i_Player)
            {
                errorType = eErrorTypes.IsNotYourPiece; // Piece does not exist or does not belong to the player
            }
            else if (!isMoveWithinBoard(i_Board, i_Move))
            {
                errorType = eErrorTypes.MoveOutOfBounds; // Move is out of bounds
            }
            else if (!piece.IsKing &&
                     ((i_Player.PlayerType == ePlayerType.Player1 && i_Move.ToRow > i_Move.FromRow) ||
                      (i_Player.PlayerType == ePlayerType.Player2 && i_Move.ToRow < i_Move.FromRow)))
            {
                errorType = eErrorTypes.CannotMoveBackwards; // Non-king piece cannot move backward
            }
            else
            {
                bool isMandatoryCapture = anyMandatoryCapture(i_Board, i_Player);
                bool isCaptureMoveFlag = isCaptureMove(i_Board, piece, i_Move);
                bool isFurtherCapture= IsFurtherCapturePossible(i_Board, i_Player, i_Move);

                if(isFurtherCapture)
                {
                    errorType = eErrorTypes.InvalidFurtherCapture;
                }

                else if (isMandatoryCapture && !isCaptureMoveFlag)
                {
                    errorType = eErrorTypes.MandatoryCaptureRequired; // A mandatory capture move exists
                }
                else if (isCaptureMoveFlag && !piece.IsKing &&
                         ((i_Player.PlayerType == ePlayerType.Player1 && i_Move.ToRow > i_Move.FromRow) ||
                          (i_Player.PlayerType == ePlayerType.Player2 && i_Move.ToRow < i_Move.FromRow)))
                {
                    errorType = eErrorTypes.OnlyKingCanEatBackwards; // Only kings can capture backwards
                }
                else if (!IsValidMove(i_Board, i_Player, i_Move))
                {
                    errorType = eErrorTypes.InvalidMove; // The move is not valid for other reasons
                }
            }

            return errorType;
        }

        public void SplitMoveInput(string i_MoveInput, out char io_FromRow, out char io_FromCol, out char io_ToRow, out char io_ToCol)
        {
            io_FromRow = i_MoveInput[0];
            io_FromCol = i_MoveInput[1];
            io_ToRow = i_MoveInput[3];
            io_ToCol = i_MoveInput[4];
        }

        public Move BuildMoveFromInput(string i_MoveInput)
        {
            bool isMoveCapture = false;
            SplitMoveInput(i_MoveInput, out char fromRow, out char fromCol, out char toRow, out char toCol);

            return new Move(fromRow - 'A', fromCol - 'a', toRow - 'A', toCol - 'a', isMoveCapture);
        }
    }
}

