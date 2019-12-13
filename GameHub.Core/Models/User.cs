using System.Runtime.Serialization;

namespace GameHub.Web.Models
{
    public class User
    {
        [IgnoreDataMemberAttribute]
        public string Id;
        public string Username;
    }
}