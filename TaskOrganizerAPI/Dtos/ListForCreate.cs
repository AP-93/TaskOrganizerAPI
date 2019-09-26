using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskOrganizerAPI.Dtos
{
    public class ListForCreate
    {
        public int userId { get; set; }
        public int boardId { get; set; }
        public string ListName { get; set; }
    }
}
