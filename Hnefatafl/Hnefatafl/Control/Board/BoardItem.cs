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
    public partial class BoardItem : System.Windows.Forms.Control
    {
        #region 定数
        protected const float MARGINE_ = (float)2.5;

        public static readonly Color SELECTED_COLOR = Color.LightGreen;

        
        #endregion

        #region 列挙型

        public enum ePieceMode
        {
            Black,
            White,
            WhiteKing,
            PointPiece
        }

        #endregion

        #region 変数, インスタンス

        protected ePieceMode p_PieceMode = ePieceMode.Black;

        //private Board m_Board = null;

        protected int p_RowIndex = 1;
        protected int p_ColumnIndex = 1;
        protected bool p_IsSelected = false;
        //protected bool p_IsExistUpper = false;

        
        #endregion

        #region プロパティ

        

        public ePieceMode Mode { get { return p_PieceMode; } }

        public bool IsSelected 
        {
            get { return p_IsSelected; }
        }

        public int RowIndex { get { return p_RowIndex; } set { p_RowIndex = value; } }

        public int ColumnIndex { get { return p_ColumnIndex; } set { p_ColumnIndex = value; } }

        /// <summary>
        /// 駒が盤上にあるときTrue
        /// 取られた時False
        /// </summary>
        public bool IsExist
        {
            get { return this.Visible; }
            set { this.Visible = value; }
        }

        //public bool IsExistUpper { get { return p_IsExistUpper; } }

        #endregion

        #region メソッド
        public BoardItem()
        {
            InitializeComponent();

            this.Size = new Size(Values.MASS_AND_PEACE_LEN, Values.MASS_AND_PEACE_LEN);

           
            GraphicsPath path = new GraphicsPath();

            path.AddEllipse(MARGINE_, MARGINE_, (float)Values.MASS_AND_PEACE_LEN - MARGINE_ * 2, (float)Values.MASS_AND_PEACE_LEN - MARGINE_ * 2);

            this.Region = new Region(path);
        }

        //public void SetBoard(Board board) 
        //{
        //    m_Board = board;
        //}

        public virtual void SelectThisItem(bool isSelected) 
        {
            p_IsSelected = isSelected;
        }

        public void SetPoint(PointPiece p) 
        {
            p_RowIndex = p.RowIndex;
            p_ColumnIndex = p.ColumnIndex;
            this.Location = p.Location;

            p.PutOnPiece(this);
        }


        public void SetPoint(int rowIndex, int columnIndex, System.Drawing.Point point)
        {
            p_RowIndex = rowIndex;
            p_ColumnIndex = columnIndex;
            this.Location = point;

        }


        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            Graphics g = pe.Graphics;

            DrawMe(g);

        }

        protected virtual void DrawMe(Graphics g) 
        {
            //各継承クラスで処理
        }

        public BoardItem Copy() 
        {
            BoardItem item = null;

            switch (p_PieceMode) 
            {
                case ePieceMode.Black:
                    item = (BoardItem)new BlackPiece();
                    
                    break;
                case ePieceMode.PointPiece:
                    item = (BoardItem)new PointPiece();

                    break;
                case ePieceMode.White:
                    item = (BoardItem)new WhitePiece();

                    break;
                case ePieceMode.WhiteKing:
                    item = (BoardItem)new WhiteKing();

                    break;
            }

            item.Visible = this.Visible;

            if(item.Visible == false) 
            { 
                int a = 0; 
            }

            item.RowIndex = p_RowIndex;
            item.ColumnIndex = p_ColumnIndex;

            return item;
        }

        #endregion


    }
}
