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



        #endregion

        #region 変数, インスタンス

        /// <summary>
        /// サイズ指定
        /// </summary>
        private Values.eGameMode m_GameMode = Values.eGameMode.Hnefatafl;


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

            SetGameMode(Values.eGameMode.Hnefatafl);
        }

        /// <summary>
        /// ボードモードの設定
        /// 1マス 30×30
        /// </summary>
        /// <param name="mode"></param>
        public void SetGameMode(Values.eGameMode mode) 
        {
            m_GameMode = mode;

            int max = 1;

            //順番注意
            max = GetMax();

            m_BoardWidth = Values.MASS_AND_PEACE_LEN * max;
            m_BoadHeight = Values.MASS_AND_PEACE_LEN * max;

            int width = m_BoardWidth + 2*Values.BOARD_MARGINE;
            int height = m_BoadHeight + 2*Values.BOARD_MARGINE;

            this.Size = new Size(width, height);

            m_LeftTop = new Point(Values.BOARD_MARGINE, Values.BOARD_MARGINE);
            m_LeftBottom = new Point(Values.BOARD_MARGINE, this.Size.Height - Values.BOARD_MARGINE);
            m_RightTop = new Point(this.Size.Width - Values.BOARD_MARGINE, Values.BOARD_MARGINE);
            m_RightBottom = new Point(this.Size.Width - Values.BOARD_MARGINE, this.Size.Height - Values.BOARD_MARGINE);

            //駒の配置
            SetPiece();
        }

        /// <summary>
        /// 駒の配置
        /// </summary>
        private void SetPiece() 
        {
            SetBlackPiece();

            SetWhitePiece();
        }

        private void SetWhitePiece() 
        {
            int max = GetMax();

            int i = 0;

            switch (m_GameMode)
            {
                case Values.eGameMode.Brandubh:
                    break;
                case Values.eGameMode.ArdRi:
                    break;
                case Values.eGameMode.Tablut:
                    break;
                case Values.eGameMode.Tawlbwrdd:
                    break;
                case Values.eGameMode.Hnefatafl:
                    //12ピース+キング1ピース

                    for (i = 0; i < 12; i++) 
                    {
                        switch (i)
                        {
                            case 0:
                            case 12:
                                //1個
                                break;

                            case 1:
                            case 2:
                            case 3:
                            case 9:
                            case 10:
                            case 11:
                                //3個
                                break;    

                            default:
                                //4個
                                break;

                        }
                    }

                    Piece piece = new Piece();
                    piece.SetBoard(this);
                    piece.SetPieceMode(Piece.PieceMode.WhiteKing);

                    this.Controls.Add(piece);

                    int x = Values.BOARD_MARGINE + Values.MASS_AND_PEACE_LEN * (max - 1) / 2;
                    int y = Values.BOARD_MARGINE + Values.MASS_AND_PEACE_LEN * (max - 1) / 2;

                    piece.Location = new Point(x, y);

                    break;
                case Values.eGameMode.AleaEvangelii:
                    break;
                default:
                    break;
            }
        }
        private void SetBlackPiece() 
        {
            switch (m_GameMode)
            {
                case Values.eGameMode.Brandubh:
                    break;
                case Values.eGameMode.ArdRi:
                    break;
                case Values.eGameMode.Tablut:
                    break;
                case Values.eGameMode.Tawlbwrdd:
                    break;
                case Values.eGameMode.Hnefatafl:
                    //24ピース
                    break;
                case Values.eGameMode.AleaEvangelii:
                    break;
                default:
                    break;
            }
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

            max = GetMax();

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

            int margin = 2;

            //中央目印
            g.DrawLine(new Pen(Color.Black), leftTop.X + (max - 1) / 2 * Values.MASS_AND_PEACE_LEN + margin,
                                                    leftTop.Y + (max - 1) / 2 * Values.MASS_AND_PEACE_LEN + margin,
                                                    leftTop.X + (max - 1) / 2 * Values.MASS_AND_PEACE_LEN + margin,
                                                    leftTop.Y + (max + 1) / 2 * Values.MASS_AND_PEACE_LEN - margin);

            g.DrawLine(new Pen(Color.Black), leftTop.X + (max + 1) / 2 * Values.MASS_AND_PEACE_LEN - margin,
                                                    leftTop.Y + (max - 1) / 2 * Values.MASS_AND_PEACE_LEN + margin,
                                                    leftTop.X + (max + 1) / 2 * Values.MASS_AND_PEACE_LEN - margin,
                                                    leftTop.Y + (max + 1) / 2 * Values.MASS_AND_PEACE_LEN - margin);

            g.DrawLine(new Pen(Color.Black), leftTop.X + (max - 1) / 2 * Values.MASS_AND_PEACE_LEN + margin,
                                                    leftTop.Y + (max - 1) / 2 * Values.MASS_AND_PEACE_LEN + margin,
                                                    leftTop.X + (max + 1) / 2 * Values.MASS_AND_PEACE_LEN - margin,
                                                    leftTop.Y + (max - 1) / 2 * Values.MASS_AND_PEACE_LEN + margin);

            g.DrawLine(new Pen(Color.Black), leftTop.X + (max - 1) / 2 * Values.MASS_AND_PEACE_LEN + margin,
                                                    leftTop.Y + (max + 1) / 2 * Values.MASS_AND_PEACE_LEN - margin,
                                                    leftTop.X + (max + 1) / 2 * Values.MASS_AND_PEACE_LEN - margin,
                                                    leftTop.Y + (max + 1) / 2 * Values.MASS_AND_PEACE_LEN - margin);

            //4隅目印
            int x1 = 0;
            int y1 = 0;
            int x2 = 0;
            int y2 = 0;
            for (int k = 0; k < 4; k++) 
            {
                //横
                switch (k)
                {
                    case 0:
                        //0: 左上隅
                        x1 = leftTop.X + margin;
                        y1 = leftTop.Y + margin;
                        x2 = leftTop.X + Values.MASS_AND_PEACE_LEN - margin;
                        break;

                    case 1:
                        //1: 左下隅
                        x1 = leftTop.X + margin;
                        y1 = leftTop.Y + max * Values.MASS_AND_PEACE_LEN - margin;
                        x2 = leftTop.X + Values.MASS_AND_PEACE_LEN - margin;
                        break;

                    case 2:
                        //2: 右上隅
                        x1 = leftTop.X + (max - 1) * Values.MASS_AND_PEACE_LEN + margin;
                        y1 = leftTop.Y + margin;
                        x2 = leftTop.X + max * Values.MASS_AND_PEACE_LEN - margin;
                        break;

                    case 3:
                        //3: 右下隅
                        x1 = leftTop.X + (max - 1) * Values.MASS_AND_PEACE_LEN + margin;
                        y1 = leftTop.Y + max * Values.MASS_AND_PEACE_LEN - margin;
                        x2 = leftTop.X + max * Values.MASS_AND_PEACE_LEN - margin;
                        break;

                    default:
                        break;
                }

                y2 = y1;

                g.DrawLine(new Pen(Color.Black), x1,
                                                 y1,
                                                 x2,
                                                 y2);

                //縦
                switch (k)
                {
                    case 0:
                        //0: 左上隅
                        x1 = leftTop.X + margin;
                        y1 = leftTop.Y + margin;
                        y2 = leftTop.Y + Values.MASS_AND_PEACE_LEN - margin;
                        break;

                    case 1:
                        //1: 左下隅
                        x1 = leftTop.X + margin;
                        y1 = leftTop.Y + (max - 1) * Values.MASS_AND_PEACE_LEN + margin;
                        y2 = leftTop.Y + max * Values.MASS_AND_PEACE_LEN - margin;
                        break;

                    case 2:
                        //2: 右上隅
                        x1 = leftTop.X + max * Values.MASS_AND_PEACE_LEN - margin;
                        y1 = leftTop.Y + margin;
                        y2 = leftTop.Y + Values.MASS_AND_PEACE_LEN - margin;
                        break;

                    case 3:
                        //3: 右下隅
                        x1 = leftTop.X + max * Values.MASS_AND_PEACE_LEN - margin;
                        y1 = leftTop.Y + (max - 1) * Values.MASS_AND_PEACE_LEN + margin;
                        y2 = leftTop.Y + max * Values.MASS_AND_PEACE_LEN - margin;
                        break;

                    default:
                        break;
                }

                x2 = x1;

                g.DrawLine(new Pen(Color.Black), x1,
                                                 y1,
                                                 x2,
                                                 y2);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameMode"></param>
        /// <returns></returns>
        private int GetMax() 
        {
            int max = 0;

            switch (m_GameMode)
            {
                case Values.eGameMode.Brandubh:
                    max = 7;

                    break;
                case Values.eGameMode.ArdRi:
                    max = 7;

                    break;
                case Values.eGameMode.Hnefatafl:
                    max = 11;

                    break;
                default:
                    break;
            }

            return max;
        }

        #endregion




    }
}
