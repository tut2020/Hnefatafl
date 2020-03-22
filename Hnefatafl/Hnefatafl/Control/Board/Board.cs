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

        private PointPiece[,] m_Point = null;

        private BlackPiece[] m_Black = null;

        private WhitePiece[] m_White = null;

        private WhiteKing m_WhiteKing = null;

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

            //SetGameMode(Values.eGameMode.Hnefatafl);
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
            max = GetBoardMax();

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
            //順番注意→ポイント→駒(駒はポイントの位置情報を使用する)
            SetPoint();

            SetBlackPiece();

            SetWhitePiece();

        }

        private void SetPoint() 
        {
            int max = GetBoardMax();

            m_Point = new PointPiece[max,max];

            for (int rowI = 0; rowI < max; rowI++) 
            {
                for (int clI = 0; clI < max; clI++) 
                {
                    m_Point[rowI, clI] = new PointPiece();

                    this.Controls.Add(m_Point[rowI, clI]);

                    int x = 0;
                    int y = 0;

                    x = m_LeftTop.X + clI * Values.MASS_AND_PEACE_LEN;
                    y = m_LeftTop.Y + (max - 1 - rowI) * Values.MASS_AND_PEACE_LEN;

                    m_Point[rowI, clI].SetPoint(rowI, clI, new Point(x,y)) ;

                    m_Point[rowI, clI].Click += RecievePointClicked;
                }
            }

        }

        private void RecievePointClicked(object sender, EventArgs e)
        {
            if(PointPiece.Is_SelectedMode == false) { return; }

            PointPiece p = (PointPiece)sender;

            int max = GetBlackMax();
            int rowI = 0;
            int colI = 0;


            for (int i = 0; i < max; i++) 
            {
                if (m_Black[i].IsSelected == true) 
                {
                    //現在の場所から移動
                    rowI = m_Black[i].RowIndex;
                    colI = m_Black[i].ColumnIndex;

                    m_Black[i].RemoveFromPoint(m_Point[rowI, colI]);

                    m_Black[i].SetOnPoint(p);

                    m_Black[i].SelectThisItem(false);

                    SetPointSelectMode(false, null);

                    return;
                }
            }

            max = GetWhiteMax();

            for (int i = 0; i < max ; i++)
            {
                if (m_White[i].IsSelected == true)
                {
                    //現在の場所から移動
                    rowI = m_White[i].RowIndex;
                    colI = m_White[i].ColumnIndex;

                    m_White[i].RemoveFromPoint(m_Point[rowI, colI]);

                    m_White[i].SetOnPoint(p);

                    m_White[i].SelectThisItem(false);

                    SetPointSelectMode(false, null);

                    return;
                }
            }

            if (m_WhiteKing.IsSelected == true) 
            {
                rowI = m_WhiteKing.RowIndex;
                colI = m_WhiteKing.ColumnIndex;

                m_WhiteKing.RemoveFromPoint(m_Point[rowI, colI]);

                m_WhiteKing.SetOnPoint(p);

                m_WhiteKing.SelectThisItem(false);

                SetPointSelectMode(false, null);
            }
        }

        private void SetWhitePiece() 
        {

            int max = GetWhiteMax();
            int i = 0;
            m_White = new WhitePiece[max];

            switch (m_GameMode)
            {
                case Values.eGameMode.Brandubh:
                    break;
                case Values.eGameMode.ArdRi:
                    break;
                case Values.eGameMode.Tablut:
                    //8ピース+1キング

                    

                    for (i = 0; i < max; i++)
                    {
                        WhitePiece item = new WhitePiece();
                        this.Controls.Add(item);
                        m_White[i] = item;
                        item.BringToFront();
                        item.Click += RecieveWhitePieceClicked;
                        

                        switch (i)
                        {
                            case 0:
                                item.SetOnPoint(m_Point[2, 4]);
                                break;
                            case 1:
                                item.SetOnPoint(m_Point[3, 4]);
                                break;
                            case 2:
                                item.SetOnPoint(m_Point[4, 2]);
                                break;
                            case 3:
                                item.SetOnPoint(m_Point[4, 3]);
                                break;
                            case 4:
                                item.SetOnPoint(m_Point[4, 5]);
                                break;
                            case 5:
                                item.SetOnPoint(m_Point[4, 6]);
                                break;
                            case 6:
                                item.SetOnPoint(m_Point[5, 4]);
                                break;
                            case 7:
                                item.SetOnPoint(m_Point[6, 4]);
                                break;
                            
                        }
                    }

                    break;
                case Values.eGameMode.Tawlbwrdd:
                    

                    break;
                case Values.eGameMode.Hnefatafl:
                    

                    break;
                case Values.eGameMode.AleaEvangelii:
                    break;
                default:
                    break;
            }

            WhiteKing piece = new WhiteKing();
            piece.SetBoard(this);
            piece.Click += RecieveWhiteKingClicked;
            m_WhiteKing = piece;

            this.Controls.Add(piece);


            piece.BringToFront();

            int boardmax = GetBoardMax();

            int x = Values.BOARD_MARGINE + Values.MASS_AND_PEACE_LEN * (boardmax - 1) / 2;
            int y = Values.BOARD_MARGINE + Values.MASS_AND_PEACE_LEN * (boardmax - 1) / 2;

            piece.Location = new Point(x, y);

        }
        private void SetBlackPiece() 
        {
            int max = GetBlackMax();
            int i = 0;

            m_Black = new BlackPiece[max];

            switch (m_GameMode)
            {
                case Values.eGameMode.Brandubh:
                    break;
                case Values.eGameMode.ArdRi:
                    break;
                case Values.eGameMode.Tablut:
                    //16ピース

                    for (i = 0; i < max; i++) 
                    {
                        BlackPiece item = new BlackPiece();

                        m_Black[i] = item;

                        this.Controls.Add(item);
                        item.Click += RecieveBlackPieceClicked;
                        item.BringToFront();

                        switch (i) 
                        {
                            case 0:
                                item.SetOnPoint(m_Point[3, 0]);
                                break;
                            case 1:
                                item.SetOnPoint(m_Point[4, 0]);
                                break;
                            case 2:
                                item.SetOnPoint(m_Point[4, 1]);
                                break;
                            case 3:
                                item.SetOnPoint(m_Point[5, 0]);
                                break;
                            case 4:
                                item.SetOnPoint(m_Point[0, 3]);
                                break;
                            case 5:
                                item.SetOnPoint(m_Point[0, 4]);
                                break;
                            case 6:
                                item.SetOnPoint(m_Point[0, 5]);
                                break;
                            case 7:
                                item.SetOnPoint(m_Point[1, 4]);
                                break;
                            case 8:
                                item.SetOnPoint(m_Point[7, 4]);
                                break;
                            case 9:
                                item.SetOnPoint(m_Point[8, 3]);
                                break;
                            case 10:
                                item.SetOnPoint(m_Point[8, 4]);
                                break;
                            case 11:
                                item.SetOnPoint(m_Point[8, 5]);
                                break;
                            case 12:
                                item.SetOnPoint(m_Point[3, 8]);
                                break;
                            case 13:
                                item.SetOnPoint(m_Point[4, 7]);
                                break;
                            case 14:
                                item.SetOnPoint(m_Point[4, 8]);
                                break;
                            case 15:
                                item.SetOnPoint(m_Point[5, 8]);
                                break;
                            
                        }
                    }

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

        private void SetPointSelectMode(bool isSelectMode, BoardItem item) 
        {
            int max = GetBoardMax();

            PointPiece.Is_SelectedMode = isSelectMode;

            if (isSelectMode == true)
            {
                //選択状態の場合は選択ピースを中心に4方向に確認処理を行う。

                bool bln = true;

                for (int i = item.RowIndex + 1; i < max; i++)
                {
                    if (m_Point[i, item.ColumnIndex].IsExistUpper == true) 
                    {
                        if (bln == true) bln = false;
                    }

                    if (bln == false) 
                    {
                        m_Point[i, item.ColumnIndex].SelectThisItem(false);
                    }
                    else
                    {
                        m_Point[i, item.ColumnIndex].SelectThisItem(true);
                    }
                }

                bln = true;

                for (int i = item.RowIndex - 1; i >= 0; i--)
                {
                    if (m_Point[i, item.ColumnIndex].IsExistUpper == true)
                    {
                        if (bln == true) bln = false;
                    }

                    if (bln == false)
                    {
                        m_Point[i, item.ColumnIndex].SelectThisItem(false);
                    }
                    else
                    {
                        m_Point[i, item.ColumnIndex].SelectThisItem(true);
                    }
                }

                bln = true;

                for (int i = item.ColumnIndex + 1; i < max; i++)
                {
                    if (m_Point[item.RowIndex, i].IsExistUpper == true)
                    {
                        if (bln == true) bln = false;
                    }

                    if (bln == false)
                    {
                        m_Point[item.RowIndex, i].SelectThisItem(false);
                    }
                    else 
                    {
                        m_Point[item.RowIndex, i].SelectThisItem(true);
                    }
                }

                bln = true;

                for (int i = item.ColumnIndex - 1; i >= 0; i--)
                {
                    if (m_Point[item.RowIndex, i].IsExistUpper == true)
                    {
                        if (bln == true) bln = false;
                    }

                    if (bln == false)
                    {
                        m_Point[item.RowIndex, i].SelectThisItem(false);
                    }
                    else
                    {
                        m_Point[item.RowIndex, i].SelectThisItem(true);
                    }
                }

            }
            else 
            {
                //非選択
                for (int rowI = 0; rowI < max; rowI++)
                {
                    for (int clI = 0; clI < max; clI++)
                    { 
                            m_Point[rowI, clI].SelectThisItem(false);

                    }
                }

            }
        }

        private void RecieveWhiteKingClicked(object sender, EventArgs e)
        {

            if (sender.GetType() == typeof(WhiteKing))
            {
                WhiteKing item = (WhiteKing)sender;

                bool bln = !item.IsSelected;

                if (bln == true)
                {
                    //選択状態にする処理では他を非選択状態にする。
                    //順番注意 選択の確認→色をデフォルトに→選択対象
                    SetDefaultColor();
                }

                SetPointSelectMode(bln, (BoardItem)item);

                item.SelectThisItem(bln);
            }

        }

        private void RecieveWhitePieceClicked(object sender, EventArgs e)
        {

            if (sender.GetType() == typeof(WhitePiece))
            {
                WhitePiece item = (WhitePiece)sender;

                bool bln = !item.IsSelected;

                if (bln == true)
                {
                    //選択状態にする処理では他を非選択状態にする。
                    //順番注意 選択の確認→色をデフォルトに→選択対象
                    SetDefaultColor();

                }

                SetPointSelectMode(bln, (BoardItem)item);

                item.SelectThisItem(bln);
            }

        }

        private void RecieveBlackPieceClicked(object sender, EventArgs e) 
        {

            if (sender.GetType() == typeof(BlackPiece)) 
            {
                BlackPiece item = (BlackPiece)sender;

                bool bln = !item.IsSelected;

                if (bln == true)
                {
                    //選択状態にする処理では他を非選択状態にする。
                    //順番注意 選択の確認→色をデフォルトに→選択対象
                    SetDefaultColor();
                }

                SetPointSelectMode(bln, (BoardItem)item);

                item.SelectThisItem(bln);

            }

        }

        private void SetDefaultColor() 
        {
            for (int i = 0; i < m_Black.Length; i++) 
            {
                m_Black[i].SelectThisItem(false);
            }

            for (int i = 0; i < m_White.Length; i++)
            {
                m_White[i].SelectThisItem(false);
            }

            m_WhiteKing.SelectThisItem(false);
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

            max = GetBoardMax();

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

        private int GetBlackMax() 
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

                case Values.eGameMode.Tablut:
                    max = 16;

                    break;

                case Values.eGameMode.Hnefatafl:
                    max = 11;

                    break;
                default:
                    break;
            }

            return max;
        }

        private int GetWhiteMax()
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

                case Values.eGameMode.Tablut:
                    max = 8;

                    break;

                case Values.eGameMode.Hnefatafl:
                    max = 11;

                    break;
                default:
                    break;
            }

            return max;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameMode"></param>
        /// <returns></returns>
        private int GetBoardMax() 
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

                case Values.eGameMode.Tablut:
                    max = 9;

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
