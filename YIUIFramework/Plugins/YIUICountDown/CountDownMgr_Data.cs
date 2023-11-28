namespace YIUIFramework
{
    public partial class CountDownMgr
    {
        /// <summary>
        /// 倒计时数据
        /// </summary>
        private class CountDownData : IRefPool
        {
            /// <summary>
            /// 在倒计时管理器中的GUID
            /// </summary>
            public int Guid;

            /// <summary>
            /// 总时间  0 = 无限回调  也可以有时间 forver = true 也可以无限回调
            /// </summary>
            public double TotalTime;

            /// <summary>
            /// 间隔时间
            /// </summary>
            public double Interval;

            /// <summary>
            /// 已经过去的时间
            /// </summary>
            public double ElapseTime;

            /// <summary>
            /// 开始时间
            /// </summary>
            public double StartTime;

            /// <summary>
            /// 最后一次回调时间
            /// </summary>
            public double LastCallBackTime;

            /// <summary>
            /// 结束时间
            /// </summary>
            public double EndTime;

            /// <summary>
            /// 永久执行  总时间到过后会重置
            /// </summary>
            public bool Forever;

            /// <summary>
            /// 回调方法
            /// </summary>
            public TimerCallback TimerCallback;

            public CountDownData()
            {
            }

            public void Reset(double totalTime, double interval, TimerCallback timerCallback, bool forever = false)
            {
                var time = GetTime();
                TotalTime = totalTime;
                Interval = interval;
                ElapseTime = 0;
                StartTime = time;
                LastCallBackTime = time;
                EndTime = time + totalTime;
                Forever = forever;
                TimerCallback = timerCallback;
            }

            public void Recycle()
            {
                Guid = 0;
                TotalTime = 0;
                Interval = 0;
                ElapseTime = 0;
                StartTime = 0;
                LastCallBackTime = 0;
                EndTime = 0;
                Forever = false;
                TimerCallback = null;
            }
        }
    }
}