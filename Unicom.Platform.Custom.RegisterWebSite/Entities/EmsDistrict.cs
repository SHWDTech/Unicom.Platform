using System.ComponentModel.DataAnnotations;

namespace Unicom.Platform.Custom.RegisterWebSite.Entities
{
    public class EmsDistrict
    {
        [Key]
        public string Code { get; set; }

        public string Name { get; set; }
    }
}