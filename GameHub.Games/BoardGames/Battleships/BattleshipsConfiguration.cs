using System.Collections.Generic;
using GameHub.Games.BoardGames.Common;

namespace GameHub.Games.BoardGames.Battleships
{
    public class BattleshipsConfiguration : GameConfiguration
    {
        public ushort Rows;

        public ushort Cols;

        // also used for verifying the user has the correct amount/type of ships.
        public List<ShipModel> InitialShipLayout { get; set; }
    }  
}