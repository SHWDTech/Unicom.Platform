using System.Collections.Generic;

namespace Unicom.Task
{
    /// <summary> 
    /// 任务管理中心 
    /// 使用它可以管理一个或则多个同时运行的任务 
    /// </summary> 
    public static class TaskScheduler
    {
        private static readonly List<Task> Tasks;

        public static int Count
            => Tasks.Count;

        static TaskScheduler()
        {
            Tasks = new List<Task>();
        }

        /// <summary>
        /// 查找任务
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Task Find(string name)
        {
            lock (Tasks)
            {
                return Tasks.Find(task => task.Name == name);
            }
        }

        public static IEnumerator<Task> GetEnumerator()
        {
            lock (Tasks)
            {
                return Tasks.GetEnumerator();
            }
        }

        /// <summary> 
        /// 终止任务 
        /// </summary> 
        public static void TerminateAllTask()
        {
            lock (Tasks)
            {
                Tasks.ForEach(task => task.Close());
                Tasks.Clear();
                Tasks.TrimExcess();
            }
        }

        public static void Register(Task task)
        {
            lock (Tasks)
            {
                Tasks.Add(task);
            }
        }
        public static void Deregister(Task task)
        {
            lock (Tasks)
            {
                Tasks.Remove(task);
            }
        }
    }
}
