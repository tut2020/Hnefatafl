using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hnefatafl
{
    /// <summary>
    /// 共通する定数や変数などを管理するクラス
    /// </summary>
    public static class Values
    {
        #region　定数

        /// <summary>
        /// 1マスのサイズ
        /// </summary>
        public const int MASS_AND_PEACE_LEN = 30;

        /// <summary>
        /// ボードのマージン
        /// </summary>
        public const int BOARD_MARGINE = 20;
        #endregion

        #region 列挙型

        /// <summary>
        /// ボードモード
        /// 
        /// 名称は以下を参照
        /// https://en.wikipedia.org/wiki/Tafl_games
        /// </summary>
        public enum eGameMode
        {
            Brandubh,
            ArdRi,
            Tablut,
            Tawlbwrdd,
            Hnefatafl,
            AleaEvangelii
        }

        #endregion
    }
}
