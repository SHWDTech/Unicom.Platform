namespace MTWESensorData.DataProvider.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("data_center.dev_info")]
    public partial class dev_info
    {
        public int ID { get; set; }

        [Required]
        [StringLength(64)]
        public string DEV_NAME { get; set; }

        [Required]
        [StringLength(64)]
        public string VER { get; set; }

        [Required]
        [StringLength(64)]
        public string IP { get; set; }

        [Required]
        [StringLength(64)]
        public string MASK { get; set; }

        [Required]
        [StringLength(64)]
        public string GATEWAY { get; set; }

        [Required]
        [StringLength(64)]
        public string STATE { get; set; }

        [Required]
        [StringLength(64)]
        public string LAST { get; set; }
    }
}
