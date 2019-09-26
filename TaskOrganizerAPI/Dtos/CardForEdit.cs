using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskOrganizerAPI.Dtos
{
    public class CardForEdit
    {
            public int Id { get; set; }
            public string CardText { get; set; }

            public int CardPosition { get; set; }

            public int BoardListId { get; set; }
       
    }
}
