using System;
using System.Collections.Generic;

namespace Orbits.GeneralProject.Core.Entities
{
    public partial class ImprovementSuggestion:EntityBase
    {
        public ImprovementSuggestion()
        {
            FileUploads = new HashSet<FileUpload>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int ImprovementSuggestionStatusId { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? ModifiedByUserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
        public bool? Inactive { get; set; }
        public int ImprovementOpportunityId { get; set; }

        public virtual User? CreatedByUser { get; set; }
        public virtual ImprovementOpportunity ImprovementOpportunity { get; set; } = null!;
        public virtual ImprovementSuggestionStatu ImprovementSuggestionStatus { get; set; } = null!;
        public virtual ICollection<FileUpload> FileUploads { get; set; }
    }
}
