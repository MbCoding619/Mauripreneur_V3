using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class TimelineNotesDTO
    {
    
        
        public string Notes { get; set; }
        
        public int TimelineId { get; set; }       

        public int SmeId { get; set; }        

        public int ProfessionalId { get; set; }

        
    }
}