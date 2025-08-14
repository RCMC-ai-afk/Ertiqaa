using Orbits.GeneralProject.DTO.FileUploadDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbits.GeneralProject.DTO.ImprovementSuggestionsDtos
{
    public class ImprovementSuggestionGetByIdModel : ImprovementSuggestionBaseDto
    {
        public int Id { get; set; }
        public string? ImprovementOpportunityName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int ImprovementSuggestionStatusId { get; set; }
        public string ImprovementSuggestionStatusName { get; set; }
        public int? OpportunityTypeId { get; set; }
        public string OpportunityTypeName { get; set; }
        public ICollection<FileUploadReturnDto>? Files { get; set; }
    }
}
