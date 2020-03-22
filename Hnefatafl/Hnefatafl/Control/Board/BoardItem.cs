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

        private Board m_Board = null;

        protected int p_RowIndex = 1;
        protected int p_ColumnIndex = 1;
        protected bool p_IsSelected = false;
        protected bool p_IsExistUpper = false;
        #endregion

        #region プロパティ

        public ePieceMode Mode { get { return p_PieceMode; } }

        public bool IsSelected 
        {
            get { return p_IsSelected; }
        }

        public int RowIndex { get { return p_RowIndex; } }

        public int ColumnIndex { get { return p_ColumnIndex; } }

        public bool IsExistUpper { get { return p_IsExistUpper; } }

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

        public void SetBoard(Board board) 
        {
            m_Board = board;
        }

        public virtual void SelectThisItem(bool isSelected) 
        {
            p_IsSelected = isSelected;
        }

        public void SetOnPoint(PointPiece p) 
        {
            p_RowIndex = p.RowIndex;
            p_ColumnIndex = p.ColumnIndex;
            this.Location = p.Location;
            p.SetIsExitUpper(true);
        }

        public void RemoveFromPoint(PointPiece p) 
        {
            //
            p.SetIsExitUpper(false);
        }

        public void SetIsExitUpper(bool isExist) 
        {
            p_IsExistUpper = isExist;
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

        }

        #endregion


    }
}
