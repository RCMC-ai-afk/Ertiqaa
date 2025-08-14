using Orbits.GeneralProject.DTO.FileUploadDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbits.GeneralProject.DTO.ImprovementSuggestionsDtos
{
    public class ImprovementSuggestionUpdateDto : ImprovementSuggestionBaseDto
    {
        public int Id { get; set; }
        public List<UploadedFileDto> Files { get; set; }
    }
}
