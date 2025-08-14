using System;
using System.Collections.Generic;

namespace Orbits.GeneralProject.Core.Entities
{
    public partial class ImprovementOpportunity:EntityBase
    {
        public ImprovementOpportunity()
        {
            ImprovementSuggestions = new HashSet<ImprovementSuggestion>();
        }

        public int Id { get; set; }
        public string? Coordinates { get; set; }
        public int? OpportunityStatusId { get; set; }
        public int? OpportunityTypeId { get; set; }
        public string? Name { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? ModifiedByUserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? Inactive { get; set; }
        public int FinancialTypeId { get; set; }

        public virtual ImprovementOpportunityStatu? OpportunityStatus { get; set; }
        public virtual ImprovementOpportunityType? OpportunityType { get; set; }
        public virtual ICollection<ImprovementSuggestion> ImprovementSuggestions { get; set; }
    }
}
