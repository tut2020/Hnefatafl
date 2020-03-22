using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Hnefatafl.Control.Board
{
    public class WhitePiece : BoardItem
    {
        #region 定数

        private readonly Color DEFAULT_P = Color.White;

        private readonly Color SELECTED_P = Color.LightSkyBlue;

        #endregion

        #region プロパティ


        #endregion

        #region メソッド

        public WhitePiece() : base()
        {

            p_PieceMode = ePieceMode.White;

            this.BackColor = DEFAULT_P;
        }

        public override void SelectThisItem(bool isSelected)
        {
            base.SelectThisItem(isSelected);


            if (isSelected == true)
            {
                //選択状態
                BackColor = SELECTED_P;
            }
            else
            {
                //非選択状態
                BackColor = DEFAULT_P;
            }
        }

        protected override void DrawMe(Graphics g)
        {
            g.DrawEllipse(new Pen(new SolidBrush(this.ForeColor), 2),
                                    MARGINE_, MARGINE_,
                                    (float)Values.MASS_AND_PEACE_LEN - MARGINE_ * 2,
                                    (float)Values.MASS_AND_PEACE_LEN - MARGINE_ * 2);
        }

        #endregion
    }
}
