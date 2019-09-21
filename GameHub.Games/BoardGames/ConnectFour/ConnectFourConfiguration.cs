using System;
using System.Collections.Generic;
using System.Text;

namespace GameHub.Games.BoardGames.ConnectFour
{
    public class ConnectFourConfiguration 
    {
        public string creatorId { get; set;}

        public ushort nRows { get; set; }

        public ushort nCols { get; set; }

        public ushort winThreshold { get; set; }

        public ushort nPlayersMax { get; set; }

        public bool Validate()
        {
            return nRows <= 30 && nCols <= 30 && winThreshold <= 30 && nPlayersMax >= 2 && nPlayersMax <= 8;
        }
    }
}
