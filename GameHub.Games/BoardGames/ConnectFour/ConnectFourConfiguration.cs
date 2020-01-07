using System.Collections.Generic;
using GameHub.Games.BoardGames.Common;

namespace GameHub.Games.BoardGames.ConnectFour
{
    public class ConnectFourConfiguration : GameConfiguration
    {
        public int nRows { get; set; }

        public int nCols { get; set; }

        public int winThreshold { get; set; }

        public int nPlayersMax { get; set; }

        ///<summary>Returns dictionary containing errors. Empty means configuration is valid.</summary>
        public Dictionary<string, string> Validate()
        {
            var errors = new Dictionary<string, string>();

            if (nRows > 30)
            {
                errors.Add("nRows", "Can't have more than 30 rows");
            }
            else if (nRows < 2)
            {
                errors.Add("nRows", "Must have atleast 2 rows"); 
            }

            if (nCols > 30)
            {
                errors.Add("nCols", "Can't have more than 30 columns");
            }
            else if (nCols < 2)
            {
                errors.Add("nCols", "Must have atleast 2 columns"); 
            }

            if (winThreshold > nRows || winThreshold > nCols)
            {
                errors.Add("winThreshold", "Win threshold can't be greater than the size of the board");
            }
            else if (winThreshold < 2)
            {
                errors.Add("winThreshold", "Win threshold can't be less than 2");
            }

            if (nPlayersMax > 8)
            {
                errors.Add("nPlayersMax", "Can't have more than 8 players");
            }
            else if (nPlayersMax < 2)
            {
                errors.Add("nPlayersMax", "Can't have less than 2 players");
            }
            // A player must be able to have the same amount of turns as the win threshold to be able to win.
            // therefore its impossible for someone to win unless condition below is met.
            else if((nRows * nCols) < ((nPlayersMax * winThreshold) - nPlayersMax + 1))
            {
                errors.Add("nPlayersMax", "Winning is impossible with this many players on this size board");
            }
           
            return errors;
        }
    }
}