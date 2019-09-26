using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskOrganizerAPI.Model
{
    public class Card
    {
        public int Id { get; set; }
        public string CardText { get; set; }

        public int CardPosition { get; set; }

        public BoardList BoardList { get; set; }
        public int BoardListId { get; set; }
    }
}
