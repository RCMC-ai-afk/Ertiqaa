using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbits.GeneralProject.DTO.ImprovementOpportunityDtos
{
    public class ImprovementOpportunityBaseDto
    {
        public string? Name { get; set; }
        public int? OpportunityTypeId { get; set; }
        public int FinancialTypeId { get; set; }
    }
}
