using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbits.GeneralProject.DTO.ImprovementOpportunityDtos
{
    public class ImprovementOpportunityGeographicalScopeGetAllDto : ImprovementOpportunityGeographicalScope
    {
        public int? OpportunityStatusId { get; set; }
        public string? OpportunityStatusName { get; set; }
        public  bool? HaveAnySuggestions { get; set; }
    }
}
