using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.UpdateDTO
{
    public class TimelineStatusUpdateDTO
    {
        public int timelineId { get; set; }

        public string timelineStatus { get; set; }
    }
}