using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Hnefatafl.Control.Board
{
    public partial class Piece : System.Windows.Forms.Control
    {
        #region 定数
        const float MARGINE_ = (float)2.5;
        #endregion

        #region 列挙型

        public enum PieceMode
        {
            Black,
            White,
            WhiteKing
        }

        #endregion

        #region 変数, インスタンス

        private PieceMode m_PieceMode = PieceMode.Black;

        private Board m_Board = null;
        
        #endregion

        #region メソッド
        public Piece()
        {
            InitializeComponent();

            this.Size = new Size(Values.MASS_AND_PEACE_LEN, Values.MASS_AND_PEACE_LEN);

            GraphicsPath path = new GraphicsPath();

            path.AddEllipse(MARGINE_, MARGINE_, (float)Values.MASS_AND_PEACE_LEN - MARGINE_ * 2, (float)Values.MASS_AND_PEACE_LEN - MARGINE_ * 2);

            this.Region = new Region(path);

            SetPieceMode(PieceMode.Black);
        }

        public void SetBoard(Board board) 
        {
            m_Board = board;
        }

        public void SetPieceMode(PieceMode mode)
        {
            m_PieceMode = mode;

            switch (mode)
            {
                case PieceMode.Black:
                    this.BackColor = Color.Black;
                    break;
                case PieceMode.White:
                    this.BackColor = Color.White;
                    break;
                case PieceMode.WhiteKing:
                    this.BackColor = Color.White;
                    this.ForeColor = Color.Black;
                    break;
                default:
                    break;
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            Graphics g = pe.Graphics;

            switch (m_PieceMode)
            {
                case PieceMode.Black:

                    break;

                case PieceMode.White:

                    g.DrawEllipse(new Pen(new SolidBrush(this.ForeColor), 2),
                                    MARGINE_, MARGINE_,
                                    (float)Values.MASS_AND_PEACE_LEN - MARGINE_ * 2,
                                    (float)Values.MASS_AND_PEACE_LEN - MARGINE_ * 2);

                    break;

                case PieceMode.WhiteKing:


                    string str = "K";

                    float x = (float)(this.Size.Width - g.MeasureString(str, this.Font).Width + MARGINE_) / 2;
                    float y = (float)(this.Size.Height - g.MeasureString(str, this.Font).Height + MARGINE_) / 2;

                    g.DrawString(str, this.Font, new SolidBrush(this.ForeColor), x, y);

                    g.DrawEllipse(new Pen(new SolidBrush(this.ForeColor), 2),
                                    MARGINE_, MARGINE_,
                                    (float)Values.MASS_AND_PEACE_LEN - MARGINE_ * 2,
                                    (float)Values.MASS_AND_PEACE_LEN - MARGINE_ * 2);

                    break;
            }
        }


        #endregion


    }
}
