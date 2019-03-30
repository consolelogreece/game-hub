using System;
using System.Collections.Generic;
using System.Text;

namespace GameHub.Games.BoardGames.ConnectFour
{
    public class ConnectFourConfiguration 
    {
        public ushort nRows { get; set; }

        public ushort nCols { get; set; }

        public ushort winThreshold { get; set; }

        public ushort nPlayersMax { get; set; }

        public bool Validate()
        {
            return nRows <= 100 && nCols <= 100 && winThreshold <= 100 && nPlayersMax <= 16;
        }
    }
}
