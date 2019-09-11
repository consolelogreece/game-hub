﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GameHub.Games.BoardGames.ConnectFour
{
    public interface IConnectFour
    {
        // return val is the row index
        MoveResult MakeMove(int col, string player);

        bool RegisterPlayer(string playerId, string playerNick);

        GameState GetGameState();

        bool Start(string playerId);

        string[][] GetBoardStateColors();     
    }
}
