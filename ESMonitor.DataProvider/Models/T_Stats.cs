namespace ESMonitor.DataProvider.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class T_Stats
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string StatCode { get; set; }

        public int? StatCodeUp { get; set; }

        [Required]
        [StringLength(50)]
        public string StatName { get; set; }

        [Required]
        [StringLength(20)]
        public string ChargeMan { get; set; }

        [Required]
        [StringLength(20)]
        public string Telepone { get; set; }

        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }

        [Required]
        [StringLength(30)]
        public string Department { get; set; }

        [Required]
        [StringLength(50)]
        public string Address { get; set; }

        public int Country { get; set; }

        [Required]
        [StringLength(20)]
        public string Street { get; set; }

        public double Square { get; set; }

        public DateTime ProStartTime { get; set; }

        public byte Stage { get; set; }

        [StringLength(20)]
        public string ProType { get; set; }

        public int? AlarmType { get; set; }

        public virtual T_Country T_Country { get; set; }
    }
}
