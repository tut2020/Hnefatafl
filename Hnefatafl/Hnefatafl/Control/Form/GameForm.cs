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
        public GameForm()
        {
            InitializeComponent();


        }

        private void GameForm_Load(object sender, EventArgs e)
        {

            Control.Board.Board br = new Control.Board.Board();
            br.BackColor = Color.White;

            br.SetGameMode(Values.eGameMode.Tablut);

            pnlStage.Controls.Add(br);

            br.Location = new Point(10, 10);


            //設計用にのせているコントロール削除
            pnlLog.Controls.Clear();

            br.SetLogPanel(pnlLog);
        }
    }
}
