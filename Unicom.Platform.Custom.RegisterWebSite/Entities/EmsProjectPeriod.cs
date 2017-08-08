using System.ComponentModel.DataAnnotations;

namespace Unicom.Platform.Custom.RegisterWebSite.Entities
{
    public class EmsProjectPeriod
    {
        [Key]
        public int Code { get; set; }

        public string Name { get; set; }
    }
}