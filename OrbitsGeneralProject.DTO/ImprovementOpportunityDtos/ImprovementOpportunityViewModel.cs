using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbits.GeneralProject.DTO.ImprovementOpportunityDtos
{
    public class ImprovementOpportunityViewModel :ImprovementOpportunityBaseDto
    {
        public int Id { get; set; }
        public string? OpportunityTypeName { get; set; }
        public int NumberOfSuggestions { get; set; }
        public int? OpportunityStatusId { get; set; }
        public string? OpportunityStatusName { get; set; }
    }
}
