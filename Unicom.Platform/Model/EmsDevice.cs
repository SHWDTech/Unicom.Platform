using System.ComponentModel.DataAnnotations;
using Unicom.Platform.Model.Service_References.UnicomPlatform;

namespace Unicom.Platform.Model
{
    public class EmsDevice : emsDevice
    {
        [Key]
        public virtual long Id { get; set; }

        public virtual string SystemCode { get; set; }

        public virtual bool OnTransfer { get; set; }
    }
}
