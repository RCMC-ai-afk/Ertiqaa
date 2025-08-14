using Orbits.GeneralProject.DTO.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbits.GeneralProject.DTO.ImprovementSuggestionsDtos
{
    public class ImprovementSuggestionFilteredDto : FilteredResultRequestDto
    {
        public List<int>? SuggestionStatusIds { get; set; }
    }
}
