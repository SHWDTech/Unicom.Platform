using System;

namespace Unicom.Task
{
    /// <summary> 
    /// 周期性的执行计划 
    /// </summary> 
    public class CycExecution : ISchedule
    {
        private DateTime _schedule;
        private TimeSpan _period;

        public DateTime ExecutionTime
        {
            get { return _schedule; }
            set { _schedule = value; }
        }

        /// <summary>
        /// 执行时间
        /// </summary>
        public long DueTime
        {
            get
            {
                var ms = (_schedule.Ticks - DateTime.Now.Ticks) / 10000;
                if (ms < 0)
                {
                    ms = 0;
                }
                return ms;
            }
        }

        /// <summary>
        /// 任务执行周期
        /// </summary>
        public long Period
            => _period.Ticks / 10000;

        /// <summary> 
        /// 构造函数,马上开始运行 
        /// </summary> 
        /// <param name="period">周期时间</param> 
        public CycExecution(TimeSpan period)
        {
            _schedule = DateTime.Now;
            _period = period;
        }
        /// <summary> 
        /// 构造函数，在一个将来时间开始运行 
        /// </summary> 
        /// <param name="shedule">计划执行的时间</param> 
        /// <param name="period">周期时间</param> 
        public CycExecution(DateTime shedule, TimeSpan period)
        {
            _schedule = shedule;
            _period = period;
        }
    }
}
