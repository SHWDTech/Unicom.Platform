using System;

namespace Unicom.Task
{
    /// <summary>
    /// 工作计划接口
    /// </summary>
    public interface ISchedule
    {
        /// <summary> 
        /// 返回最初计划执行时间 
        /// </summary> 
        DateTime ExecutionTime { set; get; }

        /// <summary> 
        /// 初始化执行时间于现在时间的时间刻度差 
        /// </summary> 
        long DueTime { get; }

        /// <summary> 
        /// 循环的周期 
        /// </summary> 
        long Period { get; }
    }
}
