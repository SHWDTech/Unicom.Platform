namespace MTWESensorData.DataProvider.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("data_center.sensor_data_hour")]
    public partial class sensor_data_hour
    {
        [Key]
        [Column(Order = 0)]
        public int ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string StatCode { get; set; }

        [Key]
        [Column(Order = 2)]
        public float TP { get; set; }

        [Key]
        [Column(Order = 3)]
        public float DB { get; set; }

        [Key]
        [Column(Order = 4, TypeName = "date")]
        public DateTime DataDate { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DataTime { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(10)]
        public string DataStatus { get; set; }

        [Key]
        [Column(Order = 7)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ValidDataNum { get; set; }
    }
}
