using System.ComponentModel.DataAnnotations;

namespace Unicom.Platform.Entities
{
    public class EmsProjectCategory
    {
        [Key]
        public int Code { get; set; }

        public string Name { get; set; }
    }
}