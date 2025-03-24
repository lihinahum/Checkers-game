using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ex02.Model;

namespace Ex02
{
    class GameManager
    {
        private readonly GameLogic m_GameLogic;
        private readonly GameUI m_GameUI;
        private List<Player> m_Players;
        private GameMode m_GameMode;
        public Board m_Board { get; set; }
        private Player m_CurrentPlayer;

        public GameManager(GameLogic i_GameLogic, GameUI i_GameUI)
        {
            m_GameLogic = i_GameLogic;
            m_GameUI = i_GameUI;
            m_Players = new List<Player>();
        }

        public void StartGame()
        {
            string player1Name;
            player1Name = GetPlayerName();

            m_Board = new Board(GetBoardSize());
            int numberOfPieces = m_GameLogic.GetAmountOfPiecesForPlayer(m_Board.BoardSize);

            m_Players.Add(new Player(player1Name, PlayerType.Player1, numberOfPieces, 'X'));

            m_GameMode = GetGameMode();

            if (m_GameMode == GameMode.HumanPlayer)
            {
                string player2Name = GetPlayerName();
                m_Players.Add(new Player(player2Name, PlayerType.Player2, numberOfPieces, 'O'));
            }
            else
            {
                m_Players.Add(new Player("computer", PlayerType.Computer, numberOfPieces, 'O'));
            }

            m_GameUI.DisplayMessage("GAME ON!");

            m_GameUI.PrintBoard(m_Board, m_Board.BoardSize);

            m_CurrentPlayer = GetRandomPlayer();

            RunGame();

        }
        public void RunGame()
        {
            //as long as the game is not over, run the game
            while (!m_GameLogic.IsGameOver(m_Players, m_Board))
            {
                m_GameUI.DisplayCurrentTurn(m_CurrentPlayer);

                //getting player move input, checking it, if it is valid - making the move and updating the board
                // if not valid - keep asking the player for valid move
                var (playerMove, playerMoveInput) = GetPlayerMove();
                //updating the board according to the player move 
                m_GameLogic.PlayerMove(m_Board, m_CurrentPlayer, playerMove);
                Ex02.ConsoleUtils.Screen.Clear();

                m_GameUI.PrintBoard(m_Board, m_Board.BoardSize);

                m_GameUI.DisplayLastMove(m_CurrentPlayer, playerMoveInput);

                //I dont know if creating a new position is a good solution
                //if the player cant continue capturing - we switching turns
                //Add here if last move was captureMove 
                if (!m_GameLogic.IsFurtherCapturePossible(m_Board, m_CurrentPlayer, new Position(playerMove.FromRow, playerMove.FromCol)))
                {
                    SwitchPlayer();
                }
                else
                {

                }
                






                
            }
            





            
        }
        private void SwitchPlayer()
        {
            if (m_CurrentPlayer == m_Players[0])
            {
                m_CurrentPlayer = m_Players[1];
            }
            else
            {
                m_CurrentPlayer = m_Players[0];
            }
        }

        public Player GetRandomPlayer()
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 2);
            return m_Players[randomNumber];
        }
        public string GetPlayerName()
        {
            bool isValidName = false;
            string playerName = "";

            while (!isValidName)
            {
                playerName = m_GameUI.GetPlayerNameInput();
                isValidName = m_GameLogic.IsValidNameCheck(playerName);
                if (!isValidName)
                {
                    m_GameUI.DisplayMessage("Please enter a name with a maximum of 20 letters without spaces.");
                }
            }
            return playerName;
        }
        public int GetBoardSize()
        {
            bool isValidSize = false;
            int boardSize = 0;
            string boardSizeInput = "";

            while (!isValidSize)
            {
                boardSizeInput = m_GameUI.GetBoardSizeInput();
                isValidSize = m_GameLogic.IsBoardSizeInputValid(boardSizeInput, out boardSize);
                if (!isValidSize)
                {
                    m_GameUI.DisplayMessage("Invalid board size. Please enter 1, 2, or 3.");
                }
            }
            return boardSize;
        }
        public GameMode GetGameMode()
        {
            bool isValidInput = false;
            int gameModeInput = 0;
            string input = "";

            while (!isValidInput)
            {

                input = m_GameUI.GetGameModeInput();
                isValidInput = m_GameLogic.IsGameModeInputValid(input, out gameModeInput);
                if (!isValidInput)
                {
                    m_GameUI.DisplayMessage("Invalid game mode. Please enter 1 or 2.");
                }
            }
            return (GameMode)gameModeInput;
        }
        private string GetValidatedMoveInput()
        {
            string moveInput = "";
            bool isValidInput = false;

            while (!isValidInput)
            {
                moveInput = m_GameUI.GetPlayerMoveInput();
                isValidInput = m_GameLogic.IsMoveInputValid(moveInput);
                if (!isValidInput)
                {
                    m_GameUI.DisplayMessage("Invalid input. Enter your move according to the format.");
                }
            }

            return moveInput;
        }
        private Move BuildMoveFromInput(string i_MoveInput)
        {
            SplitMoveInput(i_MoveInput, out char fromRow, out char fromCol, out char toRow, out char toCol);
            return new Move(fromRow - 'A', fromCol - 'a', toRow - 'A', toCol - 'a');
        }
        private (Move move, string moveInput) GetPlayerMove()
        {
            string moveInput = GetValidatedMoveInput();
            Move move = BuildMoveFromInput(moveInput);
            return (move, moveInput);
        }
        //dont know if it fits in this class - dont think so
        private void SplitMoveInput(string i_MoveInput, out char io_FromRow, out char io_FromCol, out char io_ToRow, out char io_ToCol)
        {
            io_FromRow = i_MoveInput[0];
            io_FromCol = i_MoveInput[1];
            io_ToRow = i_MoveInput[3];
            io_ToCol = i_MoveInput[4];
        }
    }

}
