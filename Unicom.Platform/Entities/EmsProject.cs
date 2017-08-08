using System;
using System.ComponentModel.DataAnnotations;

namespace Unicom.Platform.Entities
{
    public class EmsProject
    {
        [Key]
        [Display(Name = "注册编码")]
        public string Code { get; set; }

        [Display(Name = "报建编号")]
        public string RegisterCode { get; set; }

        [Display(Name = "工地名称")]
        public string Name { get; set; }

        [Display(Name = "所属区县")]
        public string District { get; set; }

        [Display(Name = "工程类型")]
        public int ProjectType { get; set; }

        [Display(Name = "工程性质")]
        public int ProjectCategory { get; set; }

        [Display(Name = "工程工期")]
        public int ProjectPeriod { get; set; }

        [Display(Name = "所在区域")]
        public int Region { get; set; }

        [Display(Name = "街道名称")]
        public string Street { get; set; }

        [Display(Name = "工程经度")]
        public string Longitude { get; set; }

        [Display(Name = "工程纬度")]
        public string Latitude { get; set; }

        [Display(Name = "承包商")]
        public string Contractors { get; set; }

        [Display(Name = "总负责人")]
        public string Superintendent { get; set; }

        [Display(Name = "联系电话")]
        public string Telephone { get; set; }

        [Display(Name = "工程地址")]
        public string Address { get; set; }

        [Display(Name = "占地面积")]
        public float SiteArea { get; set; }

        [Display(Name = "建筑面积")]
        public float BuildingArea { get; set; }

        [Display(Name = "启动时间")]
        public DateTime StartDate { get; set; }

        [Display(Name = "结束时间")]
        public DateTime EndDate { get; set; }

        [Display(Name = "工地工期")]
        public string Stage { get; set; }

        [Display(Name = "是否完工")]
        public bool IsCompleted { get; set; }

        [Display(Name = "工程状态")]
        public bool Status { get; set; }
    }
}