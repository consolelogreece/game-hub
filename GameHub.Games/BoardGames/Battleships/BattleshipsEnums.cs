namespace GameHub.Games.BoardGames.Battleships
{
    public enum Orientation { Vertical, Horizontal}
    
    // Untouched means no player has shot there. Missed means player has shot and there was no ship. Hit means player has shot and there was a ship.
    public enum SquareState { Untouched, Missed, Hit }

    public enum PlayerNumber { One, Two }

    public enum MoveConsequence {Illegal, Miss, Hit, HitSink }
}