using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskOrganizerAPI.Dtos
{
    public class ListToMove
    {
        public int currentBoardId { get; set; }
        public int currentListId { get; set; }
        public int moveToBoardId { get; set; }
        public string moveToBoardName { get; set; }
        public int moveToListId { get; set; }
        public int maxPositions { get; set; }
        public int userId { get; set; }

    }
}
