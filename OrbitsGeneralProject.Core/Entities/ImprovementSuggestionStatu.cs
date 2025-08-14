using System;
using System.Collections.Generic;

namespace Orbits.GeneralProject.Core.Entities
{
    public partial class ImprovementSuggestionStatu:EntityBase
    {
        public ImprovementSuggestionStatu()
        {
            ImprovementSuggestions = new HashSet<ImprovementSuggestion>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<ImprovementSuggestion> ImprovementSuggestions { get; set; }
    }
}
