using System.ComponentModel.DataAnnotations;
using Unicom.Platform.Model.UnicomPlatform;

namespace Unicom.Platform.Model
{
    public class EmsProject : emsProject
    {
        [Key]
        public virtual long Id { get; set; }

        public virtual string SystemCode { get; set; }

        // ReSharper disable once InconsistentNaming
        public virtual bool onTransfer { get; set; }
    }
}
