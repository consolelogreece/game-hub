using System.Runtime.Serialization;

namespace GameHub.Games.BoardGames.Common
{
    public class GameConfiguration
    {

        [IgnoreDataMemberAttribute]
        public string CreatorId { get; set; }
    }
}