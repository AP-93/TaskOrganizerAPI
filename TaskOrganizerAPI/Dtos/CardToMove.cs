using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskOrganizerAPI.Dtos
{
    public class CardToMove
    {
        public int currentBoardId { get; set; }
        public int currentListId { get; set; }
        public int cardId { get; set; }
        public int moveToListId { get; set; }
        public int moveToPosition { get; set; }
       
    }
}
