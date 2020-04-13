using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hnefatafl
{
    public partial class GameForm : Form
    {
        Control.Board.Board m_Board = new Control.Board.Board();

        public GameForm()
        {
            InitializeComponent();


        }

        private void GameForm_Load(object sender, EventArgs e)
        {

            m_Board.BackColor = Color.White;

            m_Board.SetGameMode(Values.eGameMode.Tablut);

            pnlStage.Controls.Add(m_Board);

            m_Board.Location = new Point(10, 10);


            //設計用にのせているコントロール削除
            pnlLog.Controls.Clear();

            m_Board.SetLogPanel(pnlLog);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //保存処理開始
            m_Board.SaveData();

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            //読み込み処理

            if (pnlLog.Controls.Count > 0) 
            {
                DialogResult dialogResult = MessageBox.Show("読み込みすると画面は上書きされますがよろしいですか?", "", MessageBoxButtons.OKCancel);

                if (dialogResult == DialogResult.OK) 
                {
                    m_Board.LoadData();
                }
            }
        }
    }
}
