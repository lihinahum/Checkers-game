using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Ex02.Model;

namespace Ex02.View
{
    partial class GameBoard
    {
        private System.ComponentModel.IContainer components = null;
        private CheckersButton[,] m_ButtonList;
        private CheckersButton m_SelectedButton;
        public PlayerAndScore m_Player1;
        public PlayerAndScore m_Player2;
        private Label m_CurrentPlayerLabel;
        private Panel m_MainPanel;
        public event Action<Piece> OnPieceSelected; 
       

        public string CurrentPlayer
        {
            get { return m_CurrentPlayerLabel.Text; }
            set { m_CurrentPlayerLabel.Text = "Current Turn: " + value; }
        }

        public void SetSelectedButtonToNull()
        {
            m_SelectedButton = null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        public void Subscribe(GameLogic i_GameLogic)
        {
            i_GameLogic.onPotentialMoves += I_GameLogic_onPotentialMoves;
        }

        public bool HideCurrentPlayerLabel
        {
            set { m_CurrentPlayerLabel.Visible = value; }
        }

        private void I_GameLogic_onPotentialMoves(List<Position> i_PotentialMoves, List<Position> i_CaptureMoves)
        {
            highlightPath(i_PotentialMoves,i_CaptureMoves);
        }

        private void highlightPath(List<Position> i_PotentialMoves, List<Position> i_CaptureMoves)
        {
            if (i_CaptureMoves.Count > 0)
            {
                foreach (Position pos in i_CaptureMoves)
                {
                    if (m_ButtonList[pos.CurrentRow, pos.CurrentCol] != null)
                    {
                        m_ButtonList[pos.CurrentRow, pos.CurrentCol].BackColor = Color.Crimson;
                    }
                }
            }
            else if (i_PotentialMoves.Count > 0)
            {
                foreach (Position pos in i_PotentialMoves)
                {
                    if (m_ButtonList[pos.CurrentRow, pos.CurrentCol] != null)
                    {
                        m_ButtonList[pos.CurrentRow, pos.CurrentCol].BackColor = Color.Aquamarine;
                    }
                }
            }
            else
            {
                return;
            }
        }
        private void InitializeComponent(string i_PlayerOneName, string i_PlayerTwoName, Board i_Board, string i_CurrentPlayer, string i_PlayerOneScore, string i_PlayerTwoScore)
        {
            this.SuspendLayout();

            int i_BoardSize = i_Board.BoardSize;
            int boardPixelSize = i_BoardSize * 50;
            int formWidth = boardPixelSize + 100;

            m_MainPanel = new Panel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Location = new Point(10, 10)
            };

            this.m_Player1 = new Ex02.View.PlayerAndScore();
            this.m_Player2 = new Ex02.View.PlayerAndScore();

            this.m_Player1.Location = new Point((formWidth / 4) - (this.m_Player1.Width / 2), 10);
            this.m_Player1.Name = "m_Player1";
            this.m_Player1.Score = i_PlayerOneScore;

            i_PlayerOneName += ": ";
            this.m_Player1.Text = i_PlayerOneName;
            this.m_Player1.Size = new System.Drawing.Size(i_PlayerOneName.Length, 57);
            this.m_Player1.AutoSize = true;

            this.m_Player2.Location = new Point((2 * formWidth / 4) - (this.m_Player2.Width / 2), 10);
            this.m_Player2.Name = "m_Player2";
            this.m_Player2.Score = i_PlayerTwoScore;
            i_PlayerTwoName += ": ";
            this.m_Player2.Text = i_PlayerTwoName;
            this.m_Player2.Size = new System.Drawing.Size(i_PlayerTwoName.Length, 57);
            this.m_Player2.AutoSize = true;

            m_CurrentPlayerLabel = new Label
            {
                Name = "m_CurrentPlayerLabel",
                Size = new System.Drawing.Size(200, 30),
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = Color.Blue,
                Text = "Current Turn: " + i_CurrentPlayer,
                Visible = true
            };

            m_MainPanel.Controls.Add(m_Player1);
            m_MainPanel.Controls.Add(m_Player2);
            m_MainPanel.Controls.Add(m_CurrentPlayerLabel);
            m_CurrentPlayerLabel.Location = new Point((formWidth / 2) - (m_CurrentPlayerLabel.Width / 2), m_Player1.Bottom + 10);
            CreateBoard(i_BoardSize, i_Board);
            this.Controls.Add(m_MainPanel);
            this.ClientSize = new Size(m_MainPanel.Width + 20, m_MainPanel.Height + 20);
            this.Name = "GameBoard";
            this.Text = "Checkers";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
        }

        private void CreateBoard(int i_BoardSize, Board i_Board)
        {
            this.m_ButtonList = new CheckersButton[i_BoardSize, i_BoardSize];

            Panel boardPanel = new Panel
            {
                Size = new Size(i_BoardSize * 50, i_BoardSize * 50),
                Location = new Point(10, m_CurrentPlayerLabel.Bottom + 20) 
            };

            for (int row = 0; row < i_BoardSize; row++)
            {
                for (int col = 0; col < i_BoardSize; col++)
                {
                    Piece piece = i_Board.GetPiece(row, col);
                    CheckersButton btn = new CheckersButton(row, col, piece);
                    btn.Location = new Point(col * btn.Size.Width, row * btn.Size.Width);
                    btn.OnButtonSelected += HandleButtonSelection;
                    m_ButtonList[row, col] = btn;
                    boardPanel.Controls.Add(btn);
                }
            }

            m_MainPanel.Controls.Add(boardPanel);
            m_MainPanel.Size = new Size(boardPanel.Width + 20, boardPanel.Bottom + 20);
        }

        private void clearBackground()
        {
            for (int row = 0; row < m_ButtonList.GetLength(0); row++)
            {
                for (int col = 0; col < m_ButtonList.GetLength(1); col++)
                {
                    if (m_ButtonList[row, col] != null)
                    {
                        m_ButtonList[row, col].BackColor = (row + col) % 2 != 0 ? Color.SandyBrown : Color.Black;
                    }
                }
            }
        }
        private void HandleButtonSelection(CheckersButton i_ClickedButton)
        {
            clearBackground();
            if (m_SelectedButton == i_ClickedButton)
            {
                m_SelectedButton.DeSelectButton();
                m_SelectedButton = null;
            }
            else if (i_ClickedButton.Piece == null && m_SelectedButton!= null) //potential move
            {
                int fromRow = m_SelectedButton.Row;
                int fromCol = m_SelectedButton.Col;
                int toRow = i_ClickedButton.Row;
                int toCol = i_ClickedButton.Col;
                notifyManagerOfMove(fromRow, fromCol, toRow, toCol);
            }
            else
            {
                if (m_SelectedButton != null)
                {
                    m_SelectedButton.DeSelectButton();
                    m_SelectedButton = null;
                }

                if (i_ClickedButton.Piece != null)
                {
                    m_SelectedButton = i_ClickedButton;

                    if (m_CurrentPlayerLabel.Text.Contains(m_SelectedButton.Piece.Owner.r_Name))
                    {
                        m_SelectedButton.SelectButton();
                        OnPieceSelected?.Invoke(m_SelectedButton.Piece);
                    }
                }
            }
        }

        public void UpdateButton(int i_Row, int i_Col, Piece i_Piece)
        {
            if (m_ButtonList[i_Row, i_Col] != null)
            {
                m_ButtonList[i_Row, i_Col].UpdatePiece(i_Piece);
            }
        }

        public void UpdatePlayerScores(List<Player> i_Players)
        {
            m_Player1.Score = i_Players[0].Points.ToString();
            m_Player2.Score = i_Players[1].Points.ToString();
        }
    }
}