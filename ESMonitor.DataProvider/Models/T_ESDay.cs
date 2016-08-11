namespace ESMonitor.DataProvider.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class T_ESDay
    {
        public long Id { get; set; }

        public int StatId { get; set; }

        public double TP { get; set; }

        public double DB { get; set; }

        public double? PM25 { get; set; }

        public double? PM100 { get; set; }

        public double? VOCs { get; set; }

        [Column(TypeName = "date")]
        public DateTime UpdateTime { get; set; }

        [Required]
        [StringLength(10)]
        public string DataStatus { get; set; }

        public int ValidDataNum { get; set; }

        public int DevId { get; set; }

        [StringLength(20)]
        public string Country { get; set; }
    }
}
