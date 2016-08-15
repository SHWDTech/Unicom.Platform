using System;
using System.Threading;

namespace Unicom.Task
{
    public class Task : IDisposable
    {
        private string _taskName;
        private Timer _timer;
        private TimerCallback _execTask;
        private readonly ISchedule _schedule;
        private DateTime _lastExecuteTime;
        private DateTime _nextExecuteTime;

        /// <summary> 
        /// 任务名称 
        /// </summary> 
        public string Name
        {
            set { _taskName = value; }
            get { return _taskName; }
        }
        /// <summary> 
        /// 执行任务的计划 
        /// </summary> 
        public ISchedule Shedule 
            => _schedule;

        /// <summary> 
        /// 该任务最后一次执行的时间 
        /// </summary> 
        public DateTime LastExecuteTime 
            => _lastExecuteTime;

        /// <summary> 
        /// 任务下执行时间 
        /// </summary> 
        public DateTime NextExecuteTime 
            => _nextExecuteTime;

        /// <summary> 
        /// 构造函数 
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="schedule">为每个任务制定一个执行计划</param> 
        public Task(TimerCallback callback, ISchedule schedule)
        {
            if (callback == null || schedule == null)
            {
                throw new ArgumentNullException();
            }
            _execTask = callback;
            _schedule = schedule;
            _execTask += Execute;
            TaskScheduler.Register(this);
        }

        /// <summary> 
        /// 任务内容 
        /// </summary> 
        /// <param name="state">任务函数参数</param> 
        private void Execute(object state)
        {
            _lastExecuteTime = DateTime.Now;
            if (_schedule.Period == Timeout.Infinite)
            {
                _nextExecuteTime = DateTime.MaxValue; //下次运行的时间不存在 
            }
            else
            {
                TimeSpan period = new TimeSpan(_schedule.Period * 1000);
                _nextExecuteTime = _lastExecuteTime + period;
            }
            if (!(_schedule is CycExecution))
            {
                Close();
            }
        }

        public void Start()
        {
            Start(null);
        }
        public void Start(object execTaskState)
        {
            if (_timer == null)
            {
                _timer = new Timer(_execTask, execTaskState, _schedule.DueTime, _schedule.Period);
            }
            else
            {
                _timer.Change(_schedule.DueTime, _schedule.Period);
            }
        }
        public void RefreshSchedule()
        {
            _timer?.Change(_schedule.DueTime, _schedule.Period);
        }

        public void Stop()
        {
            _timer?.Change(Timeout.Infinite, Timeout.Infinite);
        }

        public void Close()
        {
            ((IDisposable)this).Dispose();
        }

        void IDisposable.Dispose()
        {
            if (_execTask != null)
            {
                _taskName = null;
                if (_timer != null)
                {
                    _timer.Dispose();
                    _timer = null;
                }
                _execTask = null;
                TaskScheduler.Deregister(this);
            }
        }

        public override string ToString()
        {
            return _taskName;
        }
    }
}