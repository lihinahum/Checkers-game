using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Ex02.View
{
    public class CheckersButton : Button
    {
        public int Row { get; }
        public int Col { get; }
        public Piece Piece { get; private set; }

        public event Action<CheckersButton> OnButtonSelected;

        private static readonly Dictionary<(char Shape, bool IsKing), Image> sr_ShapeToImage =
            new Dictionary<(char, bool), Image>()
            {
                { ('X', false), Properties.Resources.blackCoin }, { ('X', true), Properties.Resources.blackKing },
                { ('O', false), Properties.Resources.whiteCoin }, { ('O', true), Properties.Resources.whiteKing },
            };

        public CheckersButton(int i_Row, int i_Col, Piece i_Piece)
        {
            this.Row = i_Row;
            this.Col = i_Col;
            this.Piece = i_Piece;

            this.Size = new Size(50, 50);
            this.Font = new Font("Arial", 16, FontStyle.Bold);
            this.BackColor = (i_Row + i_Col) % 2 != 0 ? Color.SandyBrown : Color.Black;
            this.Location = new Point(i_Col * 50, i_Row * 50);
            this.Enabled = (i_Row + i_Col) % 2 != 0 ? true : false;
            this.Click += (sender, e) => OnButtonSelected?.Invoke(this); 
            UpdatePiece(i_Piece);
        }

        public void SelectButton()
        {
            if (this.Piece != null)
            {
                this.BackColor = Color.CornflowerBlue;
            }
        }

        public void DeSelectButton()
        {
            this.BackColor = Color.SandyBrown;
        }

        private Image getPieceImage(Piece i_Piece)
        {
            Image pieceImage = null;

            if (i_Piece != null)
            {
                (char shape, bool king) key = (i_Piece.Owner.PieceShape, i_Piece.IsKing);
                pieceImage = sr_ShapeToImage[key];
            }
            
            return pieceImage;
        }

        public void UpdatePiece(Piece i_NewPiece)
        {
            this.Piece = i_NewPiece;
            this.Text = String.Empty;
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (this.Piece != null)
            {
                Image pieceImage = getPieceImage(this.Piece);

                if (pieceImage != null)
                {
                    int margin = 5;
                    Rectangle rect = new Rectangle(margin, margin, this.Width - 2 * margin, this.Height - 2 * margin);

                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.AddEllipse(rect);
                        e.Graphics.SetClip(path);
                        e.Graphics.DrawImage(pieceImage, rect);
                        e.Graphics.ResetClip();
                    }
                }
            }
        }
    }
}
