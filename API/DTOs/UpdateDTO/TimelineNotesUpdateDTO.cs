using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.UpdateDTO
{
    public class TimelineNotesUpdateDTO
    {
        public int timelineNotesId { get; set; }

        public string timelineNotes { get; set; }
    }
}