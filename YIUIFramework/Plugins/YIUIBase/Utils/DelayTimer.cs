using System;
using System.Collections.Generic;
using UnityEngine;

namespace YIUIFramework
{
    /// <summary>
    /// 用于只等待一次的延迟计时器。
    /// </summary>
    public sealed class DelayTimer : IDisposable
    {
        LinkedListNode<Action> updateHandle;
        float delayTime;
        float leftTime;
        Action task;

        /// <summary>
        /// 在指定秒后调用任务
        /// </summary>
        /// <param name="delay">调用任务的时间</param>
        /// <param name="task">要执行的任务</param>
        public static DelayTimer Delay(float delay, Action task)
        {
            var timer = new DelayTimer();
            timer.delayTime = delay;
            timer.task = task;
            timer.Start();
            return timer;
        }

        public void Dispose()
        {
            SchedulerMgr.RemoveFrameListener(updateHandle);
            updateHandle = null;
        }

        void Start()
        {
            leftTime = delayTime;
            updateHandle = SchedulerMgr.AddFrameListener(Update);
        }

        void Update()
        {
            leftTime -= Time.deltaTime;
            if (leftTime <= 0.0f)
            {
                try
                {
                    task?.Invoke();
                }
                catch (Exception e)
                {
                    Logger.LogError(e);
                }
                finally
                {
                    Dispose();
                }
            }
        }
    }
}