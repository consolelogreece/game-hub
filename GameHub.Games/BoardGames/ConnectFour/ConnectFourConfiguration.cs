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
            if ((nRows * nCols) < ((nPlayersMax * winThreshold) - nPlayersMax - 1))
            {
                errors.Add("General", "With this configuration, it is impossible for anyone to win.");
            }
           
            return errors;
        }
    }
}