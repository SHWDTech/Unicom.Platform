using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Unicom.Platform.Custom.RegisterWebSite.Entities;

namespace Unicom.Platform.Custom.RegisterWebSite.Models
{
    public class ProjectModel : EmsProject
    {
        [NotMapped]
        public List<EmsDistrict> Districts { get; set; }

        [NotMapped]
        public List<EmsProjectType> ProjectTypes { get; set; }

        [NotMapped]
        public List<EmsProjectCategory> ProjectCategories { get; set; }

        [NotMapped]
        public List<EmsProjectPeriod> ProjectPeriods { get; set; }

        [NotMapped]
        public List<EmsRegion> Regions { get; set; }
    }
}