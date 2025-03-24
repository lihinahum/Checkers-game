using Ex02.Model.enums;
using Ex02.Model;
using Ex02.View;
using Ex02;
using System.Collections.Generic;
using System.Windows.Forms;
using System;

public class GameManager
{
    private readonly GameLogic r_GameLogic;
    private readonly GameUI r_GameUI;
    private Player m_CurrentPlayer;
    private eGameResults m_GameResult;
    private List<Player> m_Players;
    private eGameMode m_GameMode;
    private Board m_Board;

    public GameManager(GameLogic i_GameLogic, GameUI i_GameUI)
    {
        r_GameLogic = i_GameLogic;
        r_GameUI = i_GameUI;
        m_Players = new List<Player>();
        r_GameUI.GameSettings = new GameSettings();
    }

    public void InitiateGame()
    {
        DialogResult gameSettingsDialogResult = r_GameUI.GameSettings.ShowDialog();

        if (gameSettingsDialogResult == DialogResult.OK)
        {
            SetupPlayers();
            setupBoard();
            StartGame();
        }
    }

    private void SetupPlayers()
    {
        string playerOneName = r_GameUI.GameSettings.SelectedPlayerOneName;
        string playerTwoName = r_GameUI.GameSettings.SelectedPlayerTwoName;
        int numberOfPieces = r_GameLogic.GetAmountOfPiecesForPlayer(r_GameUI.GameSettings.SelectedBoardSize);

        m_Players.Add(new Player(playerOneName, ePlayerType.Player1, numberOfPieces, 'X'));
        m_GameMode = playerTwoName == "[Computer]" ? eGameMode.Computer : eGameMode.HumanPlayer;

        if (m_GameMode == eGameMode.HumanPlayer)
        {
            m_Players.Add(new Player(playerTwoName, ePlayerType.Player2, numberOfPieces, 'O'));
        }
        else
        {
            m_Players.Add(new ComputerPlayer("[Computer]", ePlayerType.Computer, numberOfPieces, 'O'));
        }
    }

    private void i_Piece_OnPieceSelected(Piece i_Piece)
    {
        r_GameLogic.CheckAvailableMovesForSelectedPiece(m_Board,i_Piece);
    }

    private void setupBoard()
    {
        m_Board = new Board(r_GameUI.GameSettings.SelectedBoardSize);
        m_Board.InitBoard(m_Players[0], m_Players[1]);
    }

    public void StartGame()
    {
        m_CurrentPlayer = GetRandomPlayer();
        r_GameUI.GameBoard = new GameBoard(m_Players[0].GetPlayerName(), m_Players[1].GetPlayerName(), m_Board, m_CurrentPlayer.r_Name,
            m_Players[0].Points, m_Players[1].Points);
        r_GameUI.GameBoard.OnMoveSelected += i_MoveInput_OnMoveSelected;
        r_GameUI.GameBoard.OnPieceSelected += i_Piece_OnPieceSelected;
        r_GameUI.GameBoard.Subscribe(r_GameLogic);
        m_GameResult = eGameResults.Ongoing;

        if (m_CurrentPlayer.PlayerType == ePlayerType.Computer)
        {
            processComputerTurn();
        }

        r_GameUI.GameBoard.ShowDialog();
    }

    private void i_MoveInput_OnMoveSelected(string i_MoveInput)
    {
        if (m_GameResult != eGameResults.Ongoing)
        {
            return;
        }

        if (m_CurrentPlayer.PlayerType != ePlayerType.Computer)
        {
            processPlayerMove(i_MoveInput);
        }
    }

    private void processPlayerMove(string i_MoveInput)
    {
        Move move = r_GameLogic.BuildMoveFromInput(i_MoveInput);
        eErrorTypes errorType = r_GameLogic.GetErrorType(m_Board, m_CurrentPlayer, move);

        if (errorType == eErrorTypes.None)
        {
            executeMove(move);
        }
        else
        {
            r_GameUI.DisplayErrorMessage(errorType);
            r_GameUI.GameBoard.SetSelectedButtonToNull();
        }
    }
    
    private void processComputerTurn()
    {
        if (m_GameResult == eGameResults.Ongoing)
        {
            r_GameLogic.MakeComputerMove(m_Board, (ComputerPlayer)m_CurrentPlayer, out Move computerMove);
            updateBoardDisplay();

            if (!isAdditionalCaptureAvailable(computerMove))
            {
                switchPlayer();
            }
            else
            {
                r_GameUI.GameBoard.BeginInvoke(new Action(processComputerTurn));
            }

            checkGameState();
        }
    }
    
    private void executeMove(Move i_Move)
    {
        r_GameLogic.PlayerMove(m_Board, m_CurrentPlayer, i_Move);
        updateBoardDisplay();

        if (!isAdditionalCaptureAvailable(i_Move))
        {
            switchPlayer();
            if (m_CurrentPlayer.PlayerType == ePlayerType.Computer)
            {
                r_GameUI.GameBoard.BeginInvoke(new Action(processComputerTurn));
            }
        }

        checkGameState();
    }

    private void checkGameState()
    {
        m_GameResult = r_GameLogic.DetermineGameResult(m_Board, m_Players, m_GameMode);

        if (m_GameResult != eGameResults.Ongoing)
        {
            endGame();
        }
    }

    private void switchPlayer()
    {
        m_CurrentPlayer = (m_CurrentPlayer == m_Players[0]) ? m_Players[1] : m_Players[0];
        r_GameUI.GameBoard.CurrentPlayer = m_CurrentPlayer.r_Name;
    }

    private bool isAdditionalCaptureAvailable(Move i_Move)
    {
        return i_Move.IsCaptureMove && r_GameLogic.IsFurtherCapturePossible(m_Board, m_CurrentPlayer, i_Move);
    }

    private void updateBoardDisplay()
    {
        for (int row = 0; row < m_Board.BoardSize; row++)
        {
            for (int col = 0; col < m_Board.BoardSize; col++)
            {
                r_GameUI.GameBoard.UpdateButton(row, col, m_Board.GetPiece(row, col));
            }
        }

        r_GameUI.GameBoard.CurrentPlayer = m_CurrentPlayer.r_Name;
    }

    private string handleGameEnd()
    {
        string message = String.Empty;

        switch (m_GameResult)
        {
            case eGameResults.Player1Win:
                message = string.Format("{0} has won the game with a total of {1} points!", m_Players[0].r_Name, m_Players[0].Points);
                break;
            case eGameResults.Player2Win:
                message = string.Format("{0} has won the game with a total of {1} points!", m_Players[1].r_Name, m_Players[1].Points);
                break;
            case eGameResults.ComputerWin:
                message = string.Format("Computer has won the game with a total of {0} points!", m_Players[1].Points);
                break;
            case eGameResults.Tie:
                message = string.Format("It's a tie! {0} with a total of {1}," +
                                        "and {2} with a total of {3}!", m_Players[0], m_Players[0].Points,
                    m_Players[1], m_Players[1].Points);
                break;
            default:
                break;

        }

        return message;
    }

    private void endGame()
    {
        r_GameUI.GameBoard.UpdatePlayerScores(m_Players);
        r_GameUI.GameBoard.HideCurrentPlayerLabel = false;
        string endMessage = handleGameEnd();

        DialogResult playAgain = MessageBox.Show(endMessage + "\nWould you like to play another round?", 
            "Game Over", MessageBoxButtons.YesNo);

        if (playAgain == DialogResult.Yes)
        {
            InitiateAnotherRound();
            r_GameUI.GameBoard.HideCurrentPlayerLabel = true;
        }
        else
        {
            r_GameUI.GameBoard.Close();
        }
    }

    public void InitiateAnotherRound()
    {
        m_Players[0].InitializePiecesForPlayer();
        m_Players[1].InitializePiecesForPlayer();
        m_Board.ClearBoard();
        m_Board.InitBoard(m_Players[0], m_Players[1]);
        updateBoardDisplay(); 
        m_GameResult = eGameResults.Ongoing;
        r_GameUI.GameBoard.Close();
        StartGame();
    }

    public Player GetRandomPlayer()
    {
        return m_Players[r_GameLogic.m_Random.Next(0, 2)];
    }
}