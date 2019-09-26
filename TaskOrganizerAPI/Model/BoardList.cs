using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskOrganizerAPI.Model
{
    public class BoardList
    {
        public int Id { get; set; }
        public string ListName { get; set; }
        public int ListPosition { get; set; }

        public ICollection <Card> Cards { get; set; }

        public Board Board { get; set; }
        public int BoardId { get; set; }
    }
}
