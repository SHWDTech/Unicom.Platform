using System.ComponentModel.DataAnnotations;

namespace Unicom.Platform.Model
{
    public class EmsAutoDust
    {
        [Key]
        public virtual long Id { get; set; }

        public virtual string DevSystemCode { get; set; }

        public virtual long RangeMinValue { get; set; }

        public virtual long RangeMaxValue { get; set; }
    }
}
