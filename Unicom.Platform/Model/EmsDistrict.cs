﻿using System.ComponentModel.DataAnnotations;

namespace Unicom.Platform.Model
{
    public class EmsDistrict
    {
        [Key]
        public virtual long Id { get; set; }

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }
    }
}
