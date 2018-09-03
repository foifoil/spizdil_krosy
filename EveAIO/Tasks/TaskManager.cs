namespace EveAIO.Tasks
{
    using EveAIO;
    using EveAIO.Pocos;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;

    internal static class TaskManager
    {
        public static void StartTasks(List<string> ids)
        {
            TaskScheduler context;
            Helpers.SerialCheck();
            if (SynchronizationContext.Current == null)
            {
                context = TaskScheduler.Current;
            }
            else
            {
                context = TaskScheduler.FromCurrentSynchronizationContext();
            }
            ParallelOptions parallelOptions = new ParallelOptions {
                MaxDegreeOfParallelism = 10
            };
            Parallel.ForEach<string>(ids, parallelOptions, delegate (string id) {
                TaskObject task = Global.SETTINGS.TASKS.First<TaskObject>(x => x.Id == id);
                Task.Factory.StartNew(delegate {
                    if (task.State == TaskObject.StateEnum.smartWaiting)
                    {
                        task.State = TaskObject.StateEnum.running;
                    }
                    else if (task.State != TaskObject.StateEnum.multicart)
                    {
                        if (task.TaskType == TaskObject.TaskTypeEnum.manualPicker)
                        {
                            States.WriteLogger(task, States.LOGGER_STATES.MANUALPICKER_CANT_STARTED, null, "", "");
                        }
                        else
                        {
                            task.Status = States.GetTaskState(States.TaskState.STARTING);
                            task.State = TaskObject.StateEnum.running;
                            Thread thread = new Thread(new ThreadStart(new TaskRunner(task).Start));
                            thread.SetApartmentState(ApartmentState.STA);
                            task.RunnerThread = thread;
                            thread.IsBackground = true;
                            thread.Start();
                        }
                    }
                    else
                    {
                        States.WriteLogger(task, States.LOGGER_STATES.MULTICART_CANT_BE_STARTED, null, "", "");
                    }
                }).ContinueWith(delegate (Task t) {
                }, context);
            });
        }

        public static void StopTasks(List<string> ids)
        {
            ParallelOptions parallelOptions = new ParallelOptions {
                MaxDegreeOfParallelism = 10
            };
            Parallel.ForEach<string>(ids, parallelOptions, delegate (string id) {
                TaskObject task = Global.SETTINGS.TASKS.First<TaskObject>(x => x.Id == id);
                if (task.CaptchaWindow != null)
                {
                    task.CaptchaWindow.IsFree = true;
                }
                if (task.State != TaskObject.StateEnum.scheduled)
                {
                    if (((task.State != TaskObject.StateEnum.multicart) && ((task.State == TaskObject.StateEnum.running) || (task.State == TaskObject.StateEnum.waiting))) || (task.State == TaskObject.StateEnum.watching))
                    {
                        if (task.State != TaskObject.StateEnum.scheduled)
                        {
                            if (task.WatchTaskHold != null)
                            {
                                task.WatchTaskHold.Set();
                                task.WatchTaskHold = null;
                            }
                            List<string> list = new List<string>();
                            foreach (TaskObject obj2 in from x in Global.SETTINGS.TASKS
                                where x.WatchTaskId == task.Id
                                select x)
                            {
                                list.Add(obj2.Id);
                            }
                            if (list.Count > 0)
                            {
                                StopTasks(list);
                            }
                            task.State = TaskObject.StateEnum.stopped;
                            if (task.Mre != null)
                            {
                                task.Mre.Set();
                            }
                            task.RunnerThread.Abort();
                        }
                        else
                        {
                            task.State = TaskObject.StateEnum.stopped;
                        }
                        task.IsStartScheduled = false;
                        task.IsStopScheduled = false;
                        task.ShopifySmartSchedule = null;
                    }
                }
                else
                {
                    task.State = TaskObject.StateEnum.stopped;
                    task.IsStartScheduled = false;
                    task.IsStopScheduled = false;
                }
                if (Global.CAPTCHA_QUEUE.Any<TaskObject>(x => x.Id == task.Id))
                {
                    object solverLocker = Global.SolverLocker;
                    lock (solverLocker)
                    {
                        Global.CAPTCHA_QUEUE.Remove(Global.CAPTCHA_QUEUE.First<TaskObject>(x => x.Id == task.Id));
                    }
                }
            });
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly TaskManager.<>c <>9;
            public static Action<Task> <>9__0_3;
            public static Action<string> <>9__1_0;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new TaskManager.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal void <StartTasks>b__0_3(Task t)
            {
            }

            internal void <StopTasks>b__1_0(string id)
            {
                TaskObject task = Global.SETTINGS.TASKS.First<TaskObject>(x => x.Id == id);
                if (task.CaptchaWindow != null)
                {
                    task.CaptchaWindow.IsFree = true;
                }
                if (task.State != TaskObject.StateEnum.scheduled)
                {
                    if (((task.State != TaskObject.StateEnum.multicart) && ((task.State == TaskObject.StateEnum.running) || (task.State == TaskObject.StateEnum.waiting))) || (task.State == TaskObject.StateEnum.watching))
                    {
                        if (task.State != TaskObject.StateEnum.scheduled)
                        {
                            if (task.WatchTaskHold != null)
                            {
                                task.WatchTaskHold.Set();
                                task.WatchTaskHold = null;
                            }
                            List<string> ids = new List<string>();
                            foreach (TaskObject obj2 in from x in Global.SETTINGS.TASKS
                                where x.WatchTaskId == task.Id
                                select x)
                            {
                                ids.Add(obj2.Id);
                            }
                            if (ids.Count > 0)
                            {
                                TaskManager.StopTasks(ids);
                            }
                            task.State = TaskObject.StateEnum.stopped;
                            if (task.Mre != null)
                            {
                                task.Mre.Set();
                            }
                            task.RunnerThread.Abort();
                        }
                        else
                        {
                            task.State = TaskObject.StateEnum.stopped;
                        }
                        task.IsStartScheduled = false;
                        task.IsStopScheduled = false;
                        task.ShopifySmartSchedule = null;
                    }
                }
                else
                {
                    task.State = TaskObject.StateEnum.stopped;
                    task.IsStartScheduled = false;
                    task.IsStopScheduled = false;
                }
                if (Global.CAPTCHA_QUEUE.Any<TaskObject>(x => x.Id == task.Id))
                {
                    object solverLocker = Global.SolverLocker;
                    lock (solverLocker)
                    {
                        Global.CAPTCHA_QUEUE.Remove(Global.CAPTCHA_QUEUE.First<TaskObject>(x => x.Id == task.Id));
                    }
                }
            }
        }
    }
}

