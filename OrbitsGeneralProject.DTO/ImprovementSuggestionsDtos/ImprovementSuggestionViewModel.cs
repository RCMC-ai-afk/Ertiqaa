using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbits.GeneralProject.DTO.ImprovementSuggestionsDtos
{
    public class ImprovementSuggestionViewModel : ImprovementSuggestionBaseDto
    {
        public int Id { get; set; }
        public int ImprovementSuggestionStatusId { get; set; }
        public string? ImprovementSuggestionStatusName { get; set; }
        public string? ImprovementOpportunityName { get; set; }


    }
}
