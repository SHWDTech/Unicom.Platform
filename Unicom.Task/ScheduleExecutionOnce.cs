using System;
using System.Threading;

namespace Unicom.Task
{
    /// <summary> 
    /// 计划在某一未来的时间执行一个操作一次，如果这个时间比现在的时间小，就变成了立即执行的方式 
    /// </summary> 
    public class ScheduleExecutionOnce : ISchedule
    {
        private DateTime _schedule;

        public DateTime ExecutionTime
        {
            get { return _schedule; }
            set { _schedule = value; }
        }
        /// <summary> 
        /// 得到该计划还有多久才能运行 
        /// </summary> 
        public long DueTime
        {
            get
            {
                long ms = (_schedule.Ticks - DateTime.Now.Ticks) / 10000;
                if (ms < 0)
                {
                    ms = 0;
                }
                return ms;
            }
        }
        public long Period
            => Timeout.Infinite;

        /// <summary> 
        /// 构造函数 
        /// </summary>
        public ScheduleExecutionOnce(DateTime time)
        {
            _schedule = time;
        }
    }
}
