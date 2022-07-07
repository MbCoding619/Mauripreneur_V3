using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class TimelineNotes
    {
        public int TimelineNotesId { get; set; }
        
        public string Notes { get; set; }

        public int TimelineId { get; set; }

        public Timeline Timeline { get; set; }

        public int? SmeId { get; set; }

        public Sme Sme { get; set; }

        public int? ProfessionalId { get; set; }

        public Professional Professional { get; set; }
    }
}