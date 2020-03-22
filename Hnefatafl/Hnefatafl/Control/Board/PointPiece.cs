using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Hnefatafl.Control.Board
{
    public class PointPiece : BoardItem
    {
        #region 定数

        //private readonly Color DEFAULT_P = Color.Transparent;

        private readonly Color SELECTED_P = SELECTED_COLOR;

        #endregion

        #region 変数, インスタンス

        internal static bool Is_SelectedMode = false;

        #endregion

        #region プロパティ


        #endregion

        #region メソッド

        public PointPiece() : base()
        {

            p_PieceMode = ePieceMode.PointPiece;

            this.BackColor = SELECTED_P;

            this.ForeColor = Color.Black;
        }

        public override void SelectThisItem(bool isSelected)
        {
            base.SelectThisItem(isSelected);


            if (isSelected == true)
            {
                //選択状態
                Visible = true;
            }
            else
            {
                //非選択状態
                Visible = false;

            }

            this.Refresh();
        }

        protected override void DrawMe(Graphics g)
        {
            if (IsSelected == true)
            {
                //選択中
                if (BackColor != SELECTED_COLOR)
                {
                    BackColor = SELECTED_COLOR;
                }
            }
            else
            {
                //非選択
                if (BackColor != Color.Transparent)
                {
                    BackColor = Color.Transparent;
                }
            }
        }

        #endregion
    }
}
