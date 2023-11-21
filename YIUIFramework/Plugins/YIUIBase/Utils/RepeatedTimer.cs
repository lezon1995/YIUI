using System;
using System.Collections.Generic;
using UnityEngine;

namespace YIUIFramework
{
    /// <summary>
    /// 用于随时间重复的 重复计时器
    /// </summary>
    public sealed class RepeatedTimer : IDisposable
    {
        private LinkedListNode<Action> updateHandle;
        private float leftTime;
        private float repeatTime;
        private bool unscaled;
        private float speed = 1.0f;
        private Action task;

        /// <summary>
        /// 获取或设置此计时器的速度
        /// </summary>
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        /// <summary>
        /// 获取下一次触发的剩余时间
        /// </summary>
        public float LeftTime
        {
            get { return leftTime; }
        }

        /// <summary>
        /// 获取重复时间。
        /// </summary>
        public float RepeatTime
        {
            get { return repeatTime; }
        }

        /// <summary>
        /// 以指定的间隔重复调用任务
        /// </summary>
        public static RepeatedTimer Repeat(
            float interval, Action task)
        {
            var timer = new RepeatedTimer();
            timer.leftTime = interval;
            timer.repeatTime = interval;
            timer.unscaled = false;
            timer.task = task;
            timer.Start();
            return timer;
        }

        /// <summary>
        ///  以指定的间隔重复调用任务
        /// </summary>
        public static RepeatedTimer Repeat(
            float delay, float interval, Action task)
        {
            var timer = new RepeatedTimer();
            timer.leftTime = delay;
            timer.repeatTime = interval;
            timer.unscaled = false;
            timer.task = task;
            timer.Start();

            return timer;
        }

        public void Dispose()
        {
            SchedulerMgr.RemoveFrameListener(updateHandle);
            updateHandle = null;
        }

        private void Start()
        {
            updateHandle = SchedulerMgr.AddFrameListener(Update);
        }

        private void Update()
        {
            if (unscaled)
            {
                leftTime -= Time.unscaledDeltaTime * speed;
            }
            else
            {
                leftTime -= Time.deltaTime * speed;
            }

            if (leftTime <= 0.0f)
            {
                try
                {
                    task();
                }
                catch (Exception e)
                {
                    Logger.LogError(e);
                }
                finally
                {
                    leftTime = repeatTime;
                }
            }
        }
    }
}