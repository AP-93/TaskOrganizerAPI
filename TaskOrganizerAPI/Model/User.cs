using System.Collections.Generic;

namespace TaskOrganizerAPI.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PaswordHash { get; set; }
        public byte[] PaswordSalt { get; set; }

        public ICollection<UserBoard> Boards { get; set; }
    }
}
