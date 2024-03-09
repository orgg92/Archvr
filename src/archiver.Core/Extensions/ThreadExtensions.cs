namespace archiver.Core.Extensions
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public static class ThreadExtensions
    {
        public static void WaitAll(this IEnumerable<Thread> threads)
        {
            if (threads != null)
            {
                foreach (Thread thread in threads)
                {
                    thread.Join();
                }
            }
        }

        public static void WaitAllTasks(this IEnumerable<Task> tasks)
        {
            if (tasks != null)
            {
                foreach (Task task in tasks)
                {
                    task.Start();
                }
            }
        }
    }
}
