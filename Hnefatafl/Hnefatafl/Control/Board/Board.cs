using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hnefatafl.Control.Log;

namespace Hnefatafl.Control.Board
{
    public partial class Board : System.Windows.Forms.Control
    {
        #region 定数

        readonly Color BOARD_LIGHT_GRAY = Color.LightGray;
        readonly Color BOARD_WHITE = Color.White;

        const int LEFT_MARGIN = 3;
        const int TOP_MARGIN = 5;

        const int CONTROL_MARGIN = 10;

        //最大、横に500手, 縦に500手までとする。
        const int MAX_LOG = 500;

        #endregion

        #region 列挙型



        #endregion

        #region 変数, インスタンス

        /// <summary>
        /// 現在の最新の手
        /// </summary>
        private LogItem m_CurLogItem = null;

        /// <summary>
        /// ログ用パネル
        /// </summary>
        private Panel m_LogPanel = null;

        /// <summary>
        /// 襲撃者側が常に先手
        /// </summary>
        private bool m_IsAttakerTurn = true;

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

        /// <summary>
        /// 襲撃者側が常に先手
        /// </summary>
        public bool IsAttakerTurn { get { return m_IsAttakerTurn; } set { m_IsAttakerTurn = value; } }
        public PointPiece[,] Point { get { return m_Point; } set { m_Point = value; } }

        private BlackPiece[] Black { get { return m_Black; } set { m_Black = value; } }
        private WhitePiece[] White { get { return m_White; } set { m_White = value; } }

        private WhiteKing WhiteKing { get { return m_WhiteKing; } set { m_WhiteKing = value; } }

        private Values.eGameMode GameMode { get { return m_GameMode; }}

        #endregion

        #region メソッド

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Board()
        {
            InitializeComponent();

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

                    m_Point[rowI, colI].PutOnPiece(null);

                    m_Black[i].SetPoint(p);


                    m_Black[i].SelectThisItem(false);

                    SetPointSelectMode(false, null);

                    SetLogItemAndChangeTurn(BoardItem.ePieceMode.Black, rowI, colI, m_Black[i].RowIndex, m_Black[i].ColumnIndex);

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


                    m_Point[rowI, colI].PutOnPiece(null);

                    m_White[i].SetPoint(p);


                    m_White[i].SelectThisItem(false);

                    SetPointSelectMode(false, null);

                    SetLogItemAndChangeTurn(BoardItem.ePieceMode.White, rowI, colI, m_White[i].RowIndex, m_White[i].ColumnIndex);

                    return;
                }
            }

            if (m_WhiteKing.IsSelected == true) 
            {
                rowI = m_WhiteKing.RowIndex;
                colI = m_WhiteKing.ColumnIndex;

                m_Point[rowI, colI].PutOnPiece(null);

                m_WhiteKing.SetPoint(p);


                m_WhiteKing.SelectThisItem(false);

                SetPointSelectMode(false, null);

                SetLogItemAndChangeTurn(BoardItem.ePieceMode.WhiteKing, rowI, colI, m_WhiteKing.RowIndex, m_WhiteKing.ColumnIndex);
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
                                item.SetPoint(m_Point[2, 4]);
                                break;
                            case 1:
                                item.SetPoint(m_Point[3, 4]);
                                break;
                            case 2:
                                item.SetPoint(m_Point[4, 2]);
                                break;
                            case 3:
                                item.SetPoint(m_Point[4, 3]);
                                break;
                            case 4:
                                item.SetPoint(m_Point[4, 5]);
                                break;
                            case 5:
                                item.SetPoint(m_Point[4, 6]);
                                break;
                            case 6:
                                item.SetPoint(m_Point[5, 4]);
                                break;
                            case 7:
                                item.SetPoint(m_Point[6, 4]);
                                break;
                            
                        }
                    }

                    WhiteKing piece = new WhiteKing();
                    //piece.SetBoard(this);
                    piece.Click += RecieveWhiteKingClicked;
                    this.Controls.Add(piece);
                    piece.SetPoint(m_Point[4, 4]);
                    m_WhiteKing = piece;


                    piece.BringToFront();


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
                                item.SetPoint(m_Point[3, 0]);
                                break;
                            case 1:
                                item.SetPoint(m_Point[4, 0]);
                                break;
                            case 2:
                                item.SetPoint(m_Point[4, 1]);
                                break;
                            case 3:
                                item.SetPoint(m_Point[5, 0]);
                                break;
                            case 4:
                                item.SetPoint(m_Point[0, 3]);
                                break;
                            case 5:
                                item.SetPoint(m_Point[0, 4]);
                                break;
                            case 6:
                                item.SetPoint(m_Point[0, 5]);
                                break;
                            case 7:
                                item.SetPoint(m_Point[1, 4]);
                                break;
                            case 8:
                                item.SetPoint(m_Point[7, 4]);
                                break;
                            case 9:
                                item.SetPoint(m_Point[8, 3]);
                                break;
                            case 10:
                                item.SetPoint(m_Point[8, 4]);
                                break;
                            case 11:
                                item.SetPoint(m_Point[8, 5]);
                                break;
                            case 12:
                                item.SetPoint(m_Point[3, 8]);
                                break;
                            case 13:
                                item.SetPoint(m_Point[4, 7]);
                                break;
                            case 14:
                                item.SetPoint(m_Point[4, 8]);
                                break;
                            case 15:
                                item.SetPoint(m_Point[5, 8]);
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

        /// <summary>
        /// ポイントコントロールを選択状態にする
        /// </summary>
        /// <param name="isSelectMode"></param>
        /// <param name="item"></param>
        private void SetPointSelectMode(bool isSelectMode, BoardItem item) 
        {
            int max = GetBoardMax();

            PointPiece.Is_SelectedMode = isSelectMode;

            if (isSelectMode == true)
            {
                //選択状態の場合は選択ピースを中心に4方向に確認処理を行う。

                bool bln = true;

                //上方向
                for (int i = item.RowIndex + 1; i < max; i++)
                {
                    if (i == (max - 1) / 2 && item.ColumnIndex == (max - 1) / 2) { continue; }

                    if (item.Mode != BoardItem.ePieceMode.WhiteKing) 
                    {
                        if (m_Point[i, item.ColumnIndex].RowIndex == 0 && m_Point[i, item.ColumnIndex].ColumnIndex == 0) { continue; }
                        if (m_Point[i, item.ColumnIndex].RowIndex == 0 && m_Point[i, item.ColumnIndex].ColumnIndex == max - 1) { continue; }
                        if (m_Point[i, item.ColumnIndex].RowIndex == max - 1 && m_Point[i, item.ColumnIndex].ColumnIndex == 0) { continue; }
                        if (m_Point[i, item.ColumnIndex].RowIndex == max - 1 && m_Point[i, item.ColumnIndex].ColumnIndex == max - 1) { continue; }
                    }

                    if (m_Point[i, item.ColumnIndex].Piece != null) 
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

                //下方向
                for (int i = item.RowIndex - 1; i >= 0; i--)
                {
                    if (i == (max - 1) / 2 && item.ColumnIndex == (max - 1) / 2) { continue; }

                    if (item.Mode != BoardItem.ePieceMode.WhiteKing)
                    {
                        if (m_Point[i, item.ColumnIndex].RowIndex == 0 && m_Point[i, item.ColumnIndex].ColumnIndex == 0) { continue; }
                        if (m_Point[i, item.ColumnIndex].RowIndex == 0 && m_Point[i, item.ColumnIndex].ColumnIndex == max - 1) { continue; }
                        if (m_Point[i, item.ColumnIndex].RowIndex == max - 1 && m_Point[i, item.ColumnIndex].ColumnIndex == 0) { continue; }
                        if (m_Point[i, item.ColumnIndex].RowIndex == max - 1 && m_Point[i, item.ColumnIndex].ColumnIndex == max - 1) { continue; }
                    }

                    if (m_Point[i, item.ColumnIndex].Piece != null)
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

                //右方向
                for (int i = item.ColumnIndex + 1; i < max; i++)
                {
                    if (i == (max - 1) / 2 && item.RowIndex == (max - 1) / 2) { continue; }

                    if (item.Mode != BoardItem.ePieceMode.WhiteKing)
                    {
                        if (m_Point[item.RowIndex, i].RowIndex == 0 && m_Point[item.RowIndex, i].ColumnIndex == 0) { continue; }
                        if (m_Point[item.RowIndex, i].RowIndex == 0 && m_Point[item.RowIndex, i].ColumnIndex == max - 1) { continue; }
                        if (m_Point[item.RowIndex, i].RowIndex == max - 1 && m_Point[item.RowIndex, i].ColumnIndex == 0) { continue; }
                        if (m_Point[item.RowIndex, i].RowIndex == max - 1 && m_Point[item.RowIndex, i].ColumnIndex == max - 1) { continue; }
                    }

                    if (m_Point[item.RowIndex, i].Piece != null)
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

                //左方向
                for (int i = item.ColumnIndex - 1; i >= 0; i--)
                {
                    if (i == (max - 1) / 2 && item.RowIndex == (max - 1) / 2) { continue; }

                    if (item.Mode != BoardItem.ePieceMode.WhiteKing)
                    {
                        if (m_Point[item.RowIndex, i].RowIndex == 0 && m_Point[item.RowIndex, i].ColumnIndex == 0) { continue; }
                        if (m_Point[item.RowIndex, i].RowIndex == 0 && m_Point[item.RowIndex, i].ColumnIndex == max - 1) { continue; }
                        if (m_Point[item.RowIndex, i].RowIndex == max - 1 && m_Point[item.RowIndex, i].ColumnIndex == 0) { continue; }
                        if (m_Point[item.RowIndex, i].RowIndex == max - 1 && m_Point[item.RowIndex, i].ColumnIndex == max - 1) { continue; }
                    }

                    if (m_Point[item.RowIndex, i].Piece != null)
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
            if (m_IsAttakerTurn == true) { return; }

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
            if (m_IsAttakerTurn == true) { return; }

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
            if(m_IsAttakerTurn == false) { return; }

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

        /// <summary>
        /// 王を除いた王の一団(白)の最大個数
        /// </summary>
        /// <returns></returns>
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


        public void SetLogPanel(Panel logp) 
        {
            m_LogPanel = logp;

            LogItem litem = new LogItem();
            litem.Text = "開始";

            m_LogPanel.Controls.Add(litem);

            litem.Location = new Point(LEFT_MARGIN, TOP_MARGIN);

            m_CurLogItem = litem;
        }

        public void SetLogItemAndChangeTurn(BoardItem.ePieceMode mode, int curRow, int curCol, int nextRowIndex, int nextColIndex) 
        {
            LogItem item = null;
            string str = "";
            string bar = "-";
            int acsCode = 97; //65がA, 97がa



            switch (mode) 
            {
                case BoardItem.ePieceMode.Black:
                    item = new LogItem();
                    item.BackColor = Color.Black;
                    item.ForeColor = Color.White;
                    str = ((char)(acsCode + curCol)).ToString()
                        + (curRow+1)
                        + bar
                        +((char)(acsCode + nextColIndex)).ToString()
                        + (nextRowIndex+1);
                    item.Text = str;
                    m_IsAttakerTurn = false;
                    break;

                case BoardItem.ePieceMode.White:
                    item = new LogItem();
                    item.BackColor = Color.White;
                    item.ForeColor = Color.Black;
                    str = ((char)(acsCode + curCol)).ToString()
                        + (curRow + 1)
                        + bar
                        + ((char)(acsCode + nextColIndex)).ToString()
                        + (nextRowIndex + 1);
                    item.Text = str;
                    m_IsAttakerTurn = true;
                    break;

                case BoardItem.ePieceMode.WhiteKing:
                    item = new LogItem();
                    item.BackColor = Color.White;
                    item.ForeColor = Color.Black;
                    str = ((char)(acsCode + curCol)).ToString()
                        + (curRow + 1)
                        + bar
                        + ((char)(acsCode + nextColIndex)).ToString()
                        + (nextRowIndex + 1);
                    item.Text = str;
                    m_IsAttakerTurn = true;
                    break;
            }

            if (item != null) 
            {
                m_LogPanel.Controls.Add(item);

                int x = m_CurLogItem.Location.X;
                int y = m_CurLogItem.Location.Y + m_CurLogItem.Size.Height + CONTROL_MARGIN;

                item.Location = new Point(x, y);


                BoardItem[] temp = CheckRule(nextRowIndex, nextColIndex);

                int count = 0;
                string addString = "";

                //取られた駒があれば、ログへの追記を行う。
                for (int i = 0; i < temp.Length; i++) 
                {
                    if (temp[i] != null) 
                    {
                        int rowindex = temp[i].RowIndex + 1;

                        if (addString == "")
                        {
                            addString = ((char)(acsCode + temp[i].ColumnIndex)).ToString() + rowindex;
                        }
                        else 
                        {
                            addString = addString + "," + ((char)(acsCode + temp[i].ColumnIndex)).ToString() + rowindex;
                        }


                        count += 1;
                    }
                }

                if (count > 0) 
                {
                    for (int i = 0; i < count; i++) 
                    {
                        addString = "x" + addString;
                    }

                    item.Text = item.Text + addString;
                }

                item.AddPrevItem(m_CurLogItem);
                item.RecordPieceArrangement(this);

                m_CurLogItem.AddNextItem(item);
                
                m_CurLogItem = item;

            }
        }

        private BoardItem[] CheckRule(int nextRowIndex, int nextColIndex) 
        {
            BoardItem[] temp = new BoardItem[4];

            int boardMax = GetBoardMax();
            
            BoardItem cur = m_Point[nextRowIndex, nextColIndex].Piece;

            temp[0] = CheckUpper(cur, boardMax, nextRowIndex, nextColIndex);
            temp[1] = CheckLower(cur, boardMax, nextRowIndex, nextColIndex);
            temp[2] = CheckRight(cur, boardMax, nextRowIndex, nextColIndex);
            temp[3] = CheckLeft(cur, boardMax, nextRowIndex, nextColIndex);


            return temp;
        }

        /// <summary>
        /// 取られる駒がある場合はそのPieaceを返す
        /// </summary>
        /// <param name="cur"></param>
        /// <param name="boardMax"></param>
        /// <param name="nextRowIndex"></param>
        /// <param name="nextColIndex"></param>
        /// <returns></returns>
        private BoardItem CheckUpper(BoardItem cur, int boardMax, int nextRowIndex, int nextColIndex) 
        {

            BoardItem neig = null;

            BoardItem pinc = null;

            //上チェック
            if (nextRowIndex + 2 < boardMax)
            {
                neig = m_Point[nextRowIndex + 1, nextColIndex].Piece;

                pinc = m_Point[nextRowIndex + 2, nextColIndex].Piece;

                if (neig == null) { return null; }

                //上端はさみチェック
                if (nextRowIndex + 2 == boardMax - 1)
                {
                    if (nextColIndex == 0 || nextColIndex == boardMax - 1)
                    {
                        if (cur.Mode != neig.Mode)
                        {
                            neig.Visible = false;

                            m_Point[nextRowIndex + 1, nextColIndex].PutOnPiece(null);

                            return neig;
                        }
                    }
                    else 
                    {
                        //その他の場所
                        if (neig == null) return null;
                        if (pinc == null) return null;

                        if (cur.Mode == BoardItem.ePieceMode.Black)
                        {
                            if (neig.Mode != BoardItem.ePieceMode.Black && pinc.Mode == BoardItem.ePieceMode.Black)
                            {
                                neig.Visible = false;

                                m_Point[nextRowIndex + 1, nextColIndex].PutOnPiece(null);

                                return neig;
                            }
                        }
                        else
                        {
                            //白もしくはking
                            if (neig.Mode == BoardItem.ePieceMode.Black && pinc.Mode != BoardItem.ePieceMode.Black)
                            {
                                neig.Visible = false;

                                m_Point[nextRowIndex + 1, nextColIndex].PutOnPiece(null);

                                return neig;
                            }
                        }
                    }
                }
                else 
                {
                    //中央マスを使用したうえはさみチェック
                    if (nextRowIndex == (boardMax - 1) / 2 - 2 && nextColIndex == (boardMax - 1) / 2) 
                    {
                        if (cur.Mode == BoardItem.ePieceMode.Black)
                        {
                            if (neig.Mode == BoardItem.ePieceMode.White)
                            {
                                if (pinc == null)
                                {
                                    //Kingがない場合はとれる
                                    neig.Visible = false;

                                    m_Point[nextRowIndex + 1, nextColIndex].PutOnPiece(null);

                                    return neig;
                                }
                            }
                            else if (neig.Mode == BoardItem.ePieceMode.WhiteKing)
                            {

                                BoardItem left = m_Point[nextRowIndex - 1, nextColIndex - 1].Piece;

                                BoardItem right = m_Point[nextRowIndex - 1, nextColIndex + 1].Piece;

                                if (pinc == null && left.Mode == BoardItem.ePieceMode.Black && right.Mode == BoardItem.ePieceMode.Black)
                                {
                                    neig.Visible = false;

                                    m_Point[nextRowIndex + 1, nextColIndex].PutOnPiece(null);

                                    return neig;
                                }
                            }
                        }
                        else 
                        {
                            if (neig.Mode == BoardItem.ePieceMode.Black)
                            {
                                //Kingがない場合はとれる
                                neig.Visible = false;

                                m_Point[nextRowIndex + 1, nextColIndex].PutOnPiece(null);

                                return neig;
                            }
                        }
                    }
                    else
                    {
                        //その他の場所
                        if (neig == null) return null;
                        if (pinc == null) return null;

                        if (cur.Mode == BoardItem.ePieceMode.Black)
                        {
                            if (neig.Mode != BoardItem.ePieceMode.Black && pinc.Mode == BoardItem.ePieceMode.Black)
                            {
                                neig.Visible = false;

                                m_Point[nextRowIndex + 1, nextColIndex].PutOnPiece(null);

                                return neig;
                            }
                        }
                        else 
                        {
                            //白もしくはking
                            if (neig.Mode == BoardItem.ePieceMode.Black && pinc.Mode != BoardItem.ePieceMode.Black) 
                            {
                                neig.Visible = false;

                                m_Point[nextRowIndex + 1, nextColIndex].PutOnPiece(null);

                                return neig;
                            }
                        }

                        

                    }
                }

                
            }


            return null;
        }

        private BoardItem CheckLower(BoardItem cur, int boardMax, int nextRowIndex, int nextColIndex)
        {

            BoardItem neig = null;

            BoardItem pinc = null;

            //下チェック
            if (nextRowIndex - 2 >= 0)
            {
                neig = m_Point[nextRowIndex - 1, nextColIndex].Piece;

                pinc = m_Point[nextRowIndex - 2, nextColIndex].Piece;

                if (neig == null) { return null; }

                //下端はさみチェック
                if (nextRowIndex - 2 == 1)
                {
                    if (nextColIndex == 0 || nextColIndex == boardMax - 1)
                    {

                        if (cur.Mode != neig.Mode)
                        {
                            neig.Visible = false;

                            m_Point[nextRowIndex - 1, nextColIndex].PutOnPiece(null);

                            return neig;
                        }
                    }
                    else 
                    {
                        //その他の場所
                        if (neig == null) return null;
                        if (pinc == null) return null;

                        if (cur.Mode == BoardItem.ePieceMode.Black)
                        {
                            if (neig.Mode != BoardItem.ePieceMode.Black && pinc.Mode == BoardItem.ePieceMode.Black)
                            {
                                neig.Visible = false;

                                m_Point[nextRowIndex - 1, nextColIndex].PutOnPiece(null);

                                return neig;
                            }
                        }
                        else
                        {
                            //白もしくはking
                            if (neig.Mode == BoardItem.ePieceMode.Black && pinc.Mode != BoardItem.ePieceMode.Black)
                            {
                                neig.Visible = false;

                                m_Point[nextRowIndex - 1, nextColIndex].PutOnPiece(null);

                                return neig;
                            }
                        }
                    }
                }
                else
                {
                    //中央マスを使用したうえはさみチェック
                    if (nextRowIndex == (boardMax - 1) / 2 + 2 && nextColIndex == (boardMax - 1) / 2)
                    {
                        if (cur.Mode == BoardItem.ePieceMode.Black)
                        {
                            if (neig.Mode == BoardItem.ePieceMode.White)
                            {
                                if (pinc == null)
                                {
                                    //Kingがない場合はとれる
                                    neig.Visible = false;

                                    m_Point[nextRowIndex - 1, nextColIndex].PutOnPiece(null);

                                    return neig;
                                }
                            }
                            else if (neig.Mode == BoardItem.ePieceMode.WhiteKing)
                            {

                                BoardItem left = m_Point[nextRowIndex + 1, nextColIndex + 1].Piece;

                                BoardItem right = m_Point[nextRowIndex + 1, nextColIndex - 1].Piece;

                                if (pinc == null && left.Mode == BoardItem.ePieceMode.Black && right.Mode == BoardItem.ePieceMode.Black)
                                {
                                    neig.Visible = false;

                                    m_Point[nextRowIndex - 1, nextColIndex].PutOnPiece(null);

                                    return neig;
                                }
                            }
                        }
                        else
                        {
                            if (neig.Mode == BoardItem.ePieceMode.Black)
                            {
                                //Kingがない場合はとれる
                                neig.Visible = false;

                                m_Point[nextRowIndex - 1, nextColIndex].PutOnPiece(null);

                                return neig;
                            }
                        }
                    }
                    else
                    {
                        //その他の場所
                        if (neig == null) return null;
                        if (pinc == null) return null;

                        if (cur.Mode == BoardItem.ePieceMode.Black)
                        {
                            if (neig.Mode != BoardItem.ePieceMode.Black && pinc.Mode == BoardItem.ePieceMode.Black)
                            {
                                neig.Visible = false;

                                m_Point[nextRowIndex - 1, nextColIndex].PutOnPiece(null);

                                return neig;
                            }
                        }
                        else
                        {
                            //白もしくはking
                            if (neig.Mode == BoardItem.ePieceMode.Black && pinc.Mode != BoardItem.ePieceMode.Black)
                            {
                                neig.Visible = false;

                                m_Point[nextRowIndex - 1, nextColIndex].PutOnPiece(null);

                                return neig;
                            }
                        }



                    }
                }


            }


            return null;
        }

        /// <summary>
        /// 取られる駒がある場合はそのPieaceを返す
        /// </summary>
        /// <param name="cur"></param>
        /// <param name="boardMax"></param>
        /// <param name="nextRowIndex"></param>
        /// <param name="nextColIndex"></param>
        /// <returns></returns>
        private BoardItem CheckRight(BoardItem cur, int boardMax, int nextRowIndex, int nextColIndex)
        {

            BoardItem neig = null;

            BoardItem pinc = null;

            //右チェック
            if (nextColIndex + 2 < boardMax)
            {
                neig = m_Point[nextRowIndex, nextColIndex + 1].Piece;

                pinc = m_Point[nextRowIndex, nextColIndex + 2].Piece;


                if (neig == null) { return null; }

                //右端はさみチェック
                if (nextColIndex + 2 == boardMax - 1)
                {
                    if (nextRowIndex == 0 || nextRowIndex == boardMax - 1)
                    {

                        if (cur.Mode != neig.Mode)
                        {
                            neig.Visible = false;

                            m_Point[nextRowIndex, nextColIndex+1].PutOnPiece(null);

                            return neig;
                        }
                    }
                    else
                    {
                        //その他の場所
                        if (neig == null) return null;
                        if (pinc == null) return null;

                        if (cur.Mode == BoardItem.ePieceMode.Black)
                        {
                            if (neig.Mode != BoardItem.ePieceMode.Black && pinc.Mode == BoardItem.ePieceMode.Black)
                            {
                                neig.Visible = false;

                                m_Point[nextRowIndex, nextColIndex + 1].PutOnPiece(null);

                                return neig;
                            }
                        }
                        else
                        {
                            //白もしくはking
                            if (neig.Mode == BoardItem.ePieceMode.Black && pinc.Mode != BoardItem.ePieceMode.Black)
                            {
                                neig.Visible = false;

                                m_Point[nextRowIndex, nextColIndex + 1].PutOnPiece(null);

                                return neig;
                            }
                        }
                    }
                }
                else
                {
                    //中央マスを使用したうえはさみチェック
                    if (nextRowIndex == (boardMax - 1) / 2 && nextColIndex == (boardMax - 1) / 2 - 2)
                    {
                        if (cur.Mode == BoardItem.ePieceMode.Black)
                        {
                            if (neig.Mode == BoardItem.ePieceMode.White)
                            {
                                if (pinc == null)
                                {
                                    //Kingがない場合はとれる
                                    neig.Visible = false;

                                    m_Point[nextRowIndex, nextColIndex + 1].PutOnPiece(null);

                                    return neig;
                                }
                            }
                            else if (neig.Mode == BoardItem.ePieceMode.WhiteKing)
                            {

                                BoardItem left = m_Point[nextRowIndex - 1, nextColIndex - 1].Piece;

                                BoardItem right = m_Point[nextRowIndex + 1, nextColIndex  - 1].Piece;

                                if (pinc == null && left.Mode == BoardItem.ePieceMode.Black && right.Mode == BoardItem.ePieceMode.Black)
                                {
                                    neig.Visible = false;

                                    m_Point[nextRowIndex, nextColIndex + 1].PutOnPiece(null);

                                    return neig;
                                }
                            }
                        }
                        else
                        {
                            if (neig.Mode == BoardItem.ePieceMode.Black)
                            {
                                //Kingがない場合はとれる
                                neig.Visible = false;

                                m_Point[nextRowIndex, nextColIndex + 1].PutOnPiece(null);

                                return neig;
                            }
                        }
                    }
                    else
                    {
                        //その他の場所
                        if (neig == null) return null;
                        if (pinc == null) return null;

                        if (cur.Mode == BoardItem.ePieceMode.Black)
                        {
                            if (neig.Mode != BoardItem.ePieceMode.Black && pinc.Mode == BoardItem.ePieceMode.Black)
                            {
                                neig.Visible = false;

                                m_Point[nextRowIndex, nextColIndex + 1].PutOnPiece(null);

                                return neig;
                            }
                        }
                        else
                        {
                            //白もしくはking
                            if (neig.Mode == BoardItem.ePieceMode.Black && pinc.Mode != BoardItem.ePieceMode.Black)
                            {
                                neig.Visible = false;

                                m_Point[nextRowIndex, nextColIndex + 1].PutOnPiece(null);

                                return neig;
                            }
                        }



                    }
                }


            }


            return null;
        }

        private BoardItem CheckLeft(BoardItem cur, int boardMax, int nextRowIndex, int nextColIndex)
        {

            BoardItem neig = null;

            BoardItem pinc = null;

            //左チェック
            if (nextColIndex - 2 >= 0)
            {
                neig = m_Point[nextRowIndex, nextColIndex - 1].Piece;

                pinc = m_Point[nextRowIndex, nextColIndex - 2].Piece;

                if (neig == null) { return null; }

                //左端はさみチェック
                if (nextColIndex - 2 == 1)
                {
                    if (nextColIndex == 0 || nextColIndex == boardMax - 1)
                    {
                        if (cur.Mode != neig.Mode)
                        {
                            neig.Visible = false;

                            m_Point[nextRowIndex, nextColIndex - 1].PutOnPiece(null);

                            return neig;
                        }
                    }
                    else
                    {
                        //その他の場所
                        if (neig == null) return null;
                        if (pinc == null) return null;

                        if (cur.Mode == BoardItem.ePieceMode.Black)
                        {
                            if (neig.Mode != BoardItem.ePieceMode.Black && pinc.Mode == BoardItem.ePieceMode.Black)
                            {
                                neig.Visible = false;

                                m_Point[nextRowIndex, nextColIndex - 1].PutOnPiece(null);

                                return neig;
                            }
                        }
                        else
                        {
                            //白もしくはking
                            if (neig.Mode == BoardItem.ePieceMode.Black && pinc.Mode != BoardItem.ePieceMode.Black)
                            {
                                neig.Visible = false;

                                m_Point[nextRowIndex, nextColIndex - 1].PutOnPiece(null);

                                return neig;
                            }
                        }
                    }
                }
                else
                {
                    //中央マスを使用したうえはさみチェック
                    if (nextRowIndex == (boardMax - 1) / 2 && nextColIndex == (boardMax - 1) / 2 + 2)
                    {
                        if (cur.Mode == BoardItem.ePieceMode.Black)
                        {
                            if (neig.Mode == BoardItem.ePieceMode.White)
                            {
                                if (pinc == null)
                                {
                                    //Kingがない場合はとれる
                                    neig.Visible = false;

                                    m_Point[nextRowIndex, nextColIndex - 1].PutOnPiece(null);

                                    return neig;
                                }
                            }
                            else if (neig.Mode == BoardItem.ePieceMode.WhiteKing)
                            {

                                BoardItem left = m_Point[nextRowIndex + 1, nextColIndex + 1].Piece;

                                BoardItem right = m_Point[nextRowIndex - 1, nextColIndex + 1].Piece;

                                if (pinc == null && left.Mode == BoardItem.ePieceMode.Black && right.Mode == BoardItem.ePieceMode.Black)
                                {
                                    neig.Visible = false;

                                    m_Point[nextRowIndex, nextColIndex - 1].PutOnPiece(null);

                                    return neig;
                                }
                            }
                        }
                        else
                        {
                            if (neig.Mode == BoardItem.ePieceMode.Black)
                            {
                                //Kingがない場合はとれる
                                neig.Visible = false;

                                m_Point[nextRowIndex, nextColIndex - 1].PutOnPiece(null);

                                return neig;
                            }
                        }
                    }
                    else
                    {
                        //その他の場所
                        if (neig == null) return null;
                        if (pinc == null) return null;

                        if (cur.Mode == BoardItem.ePieceMode.Black)
                        {
                            if (neig.Mode != BoardItem.ePieceMode.Black && pinc.Mode == BoardItem.ePieceMode.Black)
                            {
                                neig.Visible = false;

                                m_Point[nextRowIndex, nextColIndex - 1].PutOnPiece(null);

                                return neig;
                            }
                        }
                        else
                        {
                            //白もしくはking
                            if (neig.Mode == BoardItem.ePieceMode.Black && pinc.Mode != BoardItem.ePieceMode.Black)
                            {
                                neig.Visible = false;

                                m_Point[nextRowIndex, nextColIndex - 1].PutOnPiece(null);

                                return neig;
                            }
                        }



                    }
                }


            }


            return null;
        }

        public Board Copy() 
        {
            Board board = new Board();

            board.SetGameMode(m_GameMode);

            int boardMax = GetBoardMax();
            int blackMax = GetBlackMax();
            int whiteMax = GetWhiteMax();


            ///ポイント
            for (int i = 0; i < boardMax; i++) 
            {
                for (int j = 0; j < boardMax; j++) 
                {
                    //iが行, jが列

                    board.Point[i, j] = (PointPiece)this.Point[i, j].Copy();

                }
            }

            int rowI = 0;
            int colI = 0;

            //黒
            for (int i = 0; i < blackMax; i++)
            {
                board.Black[i] = (BlackPiece)this.Black[i].Copy();

                if (board.Black[i].IsExist == true)
                {
                    rowI = board.Black[i].RowIndex;
                    colI = board.Black[i].ColumnIndex;

                    board.Point[rowI, colI].PutOnPiece((BoardItem)board.Black[i]);
                }
            }

            //白
            for (int i = 0; i < whiteMax; i++)
            {
                board.White[i] = (WhitePiece)this.White[i].Copy();

                if (board.White[i].IsExist == true)
                {
                    rowI = board.White[i].RowIndex;
                    colI = board.White[i].ColumnIndex;

                    board.Point[rowI, colI].PutOnPiece((BoardItem)board.White[i]);
                }
            }

            //王
            board.WhiteKing = (WhiteKing)this.WhiteKing.Copy();

            if (board.WhiteKing.IsExist == true)
            {
                rowI = board.WhiteKing.RowIndex;
                colI = board.WhiteKing.ColumnIndex;

                board.Point[rowI, colI].PutOnPiece((BoardItem)board.WhiteKing);
            }

            return board;
        }

        #endregion




    }
}
