namespace ESMonitor.DataProvider.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class T_ESMin
    {
        public long Id { get; set; }

        public int? StatCode { get; set; }

        public int StatId { get; set; }

        public double TP { get; set; }

        public double DB { get; set; }

        public double? PM25 { get; set; }

        public double? PM100 { get; set; }

        public double? VOCs { get; set; }

        public DateTime? UpdateTime { get; set; }

        public double WindSpeed { get; set; }

        public double? Rain { get; set; }

        public double WindDirection { get; set; }

        public double Temperature { get; set; }

        public double Humidity { get; set; }

        public double? Airpressure { get; set; }

        [Required]
        [StringLength(1)]
        public string DataStatus { get; set; }

        public int? DevId { get; set; }

        [StringLength(20)]
        public string Country { get; set; }
    }
}
