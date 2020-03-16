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

            br.SetGameMode(Values.eGameMode.Hnefatafl);

            pnlStage.Controls.Add(br);

            br.Location = new Point(10, 10);

            //Control.Board.Piece p = new Control.Board.Piece();

            //p.SetPieceMode(Control.Board.Piece.PieceMode.WhiteKing);

            //br.Controls.Add(p);

            //p.Location = new Point(Values.BOARD_MARGINE + Values.MASS_AND_PEACE_LEN * 4, Values.BOARD_MARGINE + Values.MASS_AND_PEACE_LEN * 4);


        }
    }
}
