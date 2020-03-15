using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hnefatafl.Control.Board
{
    public partial class Board : System.Windows.Forms.Control
    {
        #region 定数

        readonly Color BOARD_LIGHT_GRAY = Color.LightGray;
        readonly Color BOARD_WHITE = Color.White;

        #endregion

        #region 列挙型

        /// <summary>
        /// ボードサイズ
        /// </summary>
        public enum BoardMode
        {
            _7_7,
            _9_9,
            _11_11,
            _19_19
        }

        #endregion

        #region 変数, インスタンス

        /// <summary>
        /// サイズ指定
        /// </summary>
        private BoardMode m_BoardMode = BoardMode._11_11;


        Point m_LeftTop = new Point();
        Point m_LeftBottom = new Point();
        Point m_RightTop = new Point();
        Point m_RightBottom = new Point();

        private int m_BoardWidth = 0;
        private int m_BoadHeight = 0;
       
        #endregion

        #region プロパティ

        #endregion

        #region メソッド

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Board()
        {
            InitializeComponent();

            SetSizeMode(BoardMode._11_11);
        }

        /// <summary>
        /// ボードモードの設定
        /// 1マス 30×30
        /// </summary>
        /// <param name="mode"></param>
        public void SetSizeMode(BoardMode mode) 
        {
            m_BoardMode = mode;

            int a = 1;

            switch (m_BoardMode)
            {
                case BoardMode._7_7:
                    a = 7;
                    break;
                case BoardMode._9_9:
                    a = 9;
                    break;
                case BoardMode._11_11:
                    a = 11;
                    break;
                case BoardMode._19_19:
                    a = 19;
                    break;
                default:
                    a = 11;
                    break;
            }

            m_BoardWidth = Values.MASS_AND_PEACE_LEN * a;
            m_BoadHeight = Values.MASS_AND_PEACE_LEN * a;

            int width = m_BoardWidth + 2*Values.BOARD_MARGINE;
            int height = m_BoadHeight + 2*Values.BOARD_MARGINE;

            this.Size = new Size(width, height);

            m_LeftTop = new Point(Values.BOARD_MARGINE, Values.BOARD_MARGINE);
            m_LeftBottom = new Point(Values.BOARD_MARGINE, this.Size.Height - Values.BOARD_MARGINE);
            m_RightTop = new Point(this.Size.Width - Values.BOARD_MARGINE, Values.BOARD_MARGINE);
            m_RightBottom = new Point(this.Size.Width - Values.BOARD_MARGINE, this.Size.Height - Values.BOARD_MARGINE);

        }

        /// <summary>
        /// 描画イベント
        /// </summary>
        /// <param name="pe"></param>
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            Graphics g = pe.Graphics;

            Point leftTop = m_LeftTop;
            Point leftBottom = m_LeftBottom;
            Point rightTop = m_RightTop;
            Point rightBottom = m_RightBottom;

            int max = 0;

            switch (m_BoardMode)
            {
                case BoardMode._7_7:
                    max = 7;

                    break;
                case BoardMode._9_9:
                    max = 9;

                    break;
                case BoardMode._11_11:
                    max = 11;

                    break;
                case BoardMode._19_19:
                    max = 19;

                    break;
                default:
                    break;
            }

           

            Brush br = new SolidBrush(BOARD_LIGHT_GRAY);


            for (int colIndex = 0; colIndex < max; colIndex++) 
            {
                //列番号

                for (int rowIndex = 0; rowIndex < max; rowIndex++) 
                {
                    //行番号

                    int check = (colIndex + rowIndex) % 2;

                    if (check == 0)
                    {
                        br = new SolidBrush(BOARD_WHITE);
                    }
                    else 
                    {
                        br = new SolidBrush(BOARD_LIGHT_GRAY);
                    }

                    g.FillRectangle(br, leftTop.X + colIndex * Values.MASS_AND_PEACE_LEN, 
                                        leftTop.Y + rowIndex * Values.MASS_AND_PEACE_LEN,
                                        Values.MASS_AND_PEACE_LEN,
                                        Values.MASS_AND_PEACE_LEN);
                }
            }

            for (int i = 0; i < max + 1; i++)
            {

                float x = Values.BOARD_MARGINE / 2;
                //float y = this.Size.Height - (Values.BOARD_MARGINE + Values.MASS_AND_PEACE_LEN * 1 / 2 + Values.MASS_AND_PEACE_LEN * i);
                float y = (float)this.Size.Height - ((float)Values.BOARD_MARGINE + (float)Values.MASS_AND_PEACE_LEN * ((float)i + (float)1/2));
                int acsCode = 65; //65がA

                //行番号
                if (i < max) 
                {
                    string str = (i + 1).ToString();

                    x = x - g.MeasureString(str, this.Font).Width / 2;

                    y = y - g.MeasureString(str, this.Font).Height / 2;

                    g.DrawString(str,
                                this.Font,
                                Brushes.Black,
                                x,
                                y);

                    //アスキーコード変換
                    str = ((char)(acsCode + i)).ToString();

                    x = (float)Values.BOARD_MARGINE + (float)Values.MASS_AND_PEACE_LEN * ((float)i + (float)1 / 2);

                    x = x - g.MeasureString(str, this.Font).Width / 2;

                    y = (float)this.Size.Height - ((float)Values.BOARD_MARGINE / 2 + (float)g.MeasureString(str, this.Font).Height / 2);

                    g.DrawString(str,
                                this.Font,
                                Brushes.Black,
                                x,
                                y);
                }



                //縦
                g.DrawLine(new Pen(Color.Black), leftTop.X + i * Values.MASS_AND_PEACE_LEN,
                                                    leftTop.Y,
                                                    leftBottom.X + i * Values.MASS_AND_PEACE_LEN,
                                                    leftBottom.Y);

                //横
                g.DrawLine(new Pen(Color.Black), leftTop.X,
                                                    leftTop.Y + i * Values.MASS_AND_PEACE_LEN,
                                                    rightTop.X,
                                                    rightTop.Y + i * Values.MASS_AND_PEACE_LEN);

            }
        }

        #endregion




    }
}
