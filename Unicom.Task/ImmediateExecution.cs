using System;
using System.Threading;

namespace Unicom.Task
{
    /// <summary> 
    /// 计划立即执行任务 
    /// </summary> 
    public class ImmediateExecution : ISchedule
    {
        public DateTime ExecutionTime
        {
            get { return DateTime.Now; }
            set { }
        }
        public long DueTime
            => 0;

        public long Period
            => Timeout.Infinite;
    }
}
