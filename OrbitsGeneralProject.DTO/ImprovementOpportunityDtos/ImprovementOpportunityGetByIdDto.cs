using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbits.GeneralProject.DTO.ImprovementOpportunityDtos
{
    public class ImprovementOpportunityGetByIdDto : ImprovementOpportunityBaseDto
    {
        public int Id { get; set; }
        public string? OpportunityTypeName { get; set; }
        public string? OpportunityStatusName { get; set; }

        public int CountSuggestions { get; set; }

    }
}
