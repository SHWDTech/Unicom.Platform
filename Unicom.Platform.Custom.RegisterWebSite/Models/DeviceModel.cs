using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Unicom.Platform.Custom.RegisterWebSite.Entities;

namespace Unicom.Platform.Custom.RegisterWebSite.Models
{
    [NotMapped]
    public class DeviceModel : EmsDevice
    {
        [NotMapped]
        public List<EmsProject> Projects { get; set; }
    }
}