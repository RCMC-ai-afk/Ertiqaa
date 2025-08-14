using Orbits.GeneralProject.DTO.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbits.GeneralProject.DTO.ImprovementOpportunityDtos
{
    public class ImprovementOpportunityFilteredDto : FilteredResultRequestDto
    {
        public List<int>? Ids { get; set; }
        public List<int>? OpportunityStatusIds { get; set; }
        public List<int>? OpportunityTypeIds { get; set; }
    }
}
