namespace MTWESensorData.DataProvider.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("data_center.sensor_data_min")]
    public partial class sensor_data_min
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
        [Column(Order = 4)]
        public DateTime DataTime { get; set; }

        [Key]
        [Column(Order = 5)]
        public float WindSpeed { get; set; }

        [Key]
        [Column(Order = 6)]
        public float Rain { get; set; }

        [Key]
        [Column(Order = 7)]
        public float WindDirection { get; set; }

        [Key]
        [Column(Order = 8)]
        public float Temperature { get; set; }

        [Key]
        [Column(Order = 9)]
        public float Humidity { get; set; }

        [Key]
        [Column(Order = 10)]
        public float AirPressure { get; set; }

        [Key]
        [Column(Order = 11)]
        [StringLength(10)]
        public string DataStatus { get; set; }
    }
}
