﻿using System.ComponentModel.DataAnnotations;

namespace Unicom.Platform.Model
{
    public class EmsDevice
    {
        [Key]
        public virtual long Id { get; set; }

        public virtual string UnicomCode { get; set; }

        public virtual string SystemCode { get; set; }

        public virtual bool OnTransfer { get; set; }
    }
}