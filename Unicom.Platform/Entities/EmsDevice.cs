using System;
using System.ComponentModel.DataAnnotations;

namespace Unicom.Platform.Entities
{
    public class EmsDevice
    {
        [Key]
        [Display(Name = "注册编号")]
        public string Code { get; set; }

        [Display(Name = "设备名称")]
        public string Name { get; set; }

        [Display(Name = "设备IP地址")]
        public string IpAddr { get; set; }

        [Display(Name = "设备MAC地址")]
        public string MacAddr { get; set; }

        [Display(Name = "设备端口号")]
        public string Port { get; set; }

        [Display(Name = "设备版本")]
        public string Version { get; set; }

        [Display(Name = "工程编号")]
        public string ProjectCode { get; set; }

        [Display(Name = "设备经度")]
        public string Longitude { get; set; }

        [Display(Name = "设备纬度")]
        public string Latitude { get; set; }

        [Display(Name = "开始时间")]
        public DateTime StartDate { get; set; }

        [Display(Name = "结束时间")]
        public DateTime EndDate { get; set; }

        [Display(Name = "安装时间")]
        public DateTime InstallDate { get; set; }

        [Display(Name = "在线状态")]
        public bool OnlineStatus { get; set; }

        [Display(Name = "视频地址")]
        public string VideoUrl { get; set; }

        [Display(Name = "是否开启上传")]
        public bool IsTransfer { get; set; }

        public bool IsHandlerValues { get; set; }

        public double TpMax { get; set; }

        public double TpMin { get; set; }
    }
}