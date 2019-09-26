using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskOrganizerAPI.Dtos
{
    public class BoardForCreate
    {
        public int userId { get; set; }
        public string BoardName { get; set; }
    }
}
