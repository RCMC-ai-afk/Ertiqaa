using System;
using System.Collections.Generic;

namespace Orbits.GeneralProject.Core.Entities
{
    public partial class ImprovementOpportunityStatu:EntityBase
    {
        public ImprovementOpportunityStatu()
        {
            ImprovementOpportunities = new HashSet<ImprovementOpportunity>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<ImprovementOpportunity> ImprovementOpportunities { get; set; }
    }
}
