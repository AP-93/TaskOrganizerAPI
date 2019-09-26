using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskOrganizerAPI.Model
{
    public class Board
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public int OwnerId { get; set; }
        public ICollection<BoardList> BoardLists { get; set; }

        public ICollection<UserBoard> Users { get; set; }
    }
}
