namespace ESMonitor.DataProvider.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class T_Devs
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string DevCode { get; set; }

        [Required]
        [StringLength(20)]
        public string StatId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime PreEndTime { get; set; }

        public DateTime EndTime { get; set; }

        public byte DevStatus { get; set; }

        [StringLength(50)]
        public string VideoURL { get; set; }
    }
}
