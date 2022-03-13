using System;

using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace NikosAssets.Helpers
{
    [Serializable]
    public class TimingHelper : ICloneable
    {
        public enum TimerType
        {
            //Instant represents no check/ instant true
            Instant = 0,
            //Always false
            Never = 1,

            MilliSeconds = 3,
            Seconds = 4,
            Minutes = 5,
            Hours = 6,
            Days = 7
        }

        public TimingHelper()
        {
            this.Init();
        }

        public TimingHelper(TimerType timerType)
        {
            this.timerType = timerType;
            
            this.Init();
        }
        
        public TimingHelper(TimerType timerType, Vector2 minMaxRandomTimeRange)
        {
            this.timerType = timerType;
            this.minMaxRandomTimeRange = minMaxRandomTimeRange;
            
            this.Init();
        }
        
        public TimerType timerType = TimerType.Instant;

        private bool _HideIf_MinMaxTime() => timerType == TimerType.Instant || timerType == TimerType.Never;
        [HideIf(nameof(_HideIf_MinMaxTime))]
        [AllowNesting]
        public Vector2 minMaxRandomTimeRange = new Vector2(10, 10);
        
        public DateTime CheckAgainstTime { get; set; } = DateTime.Now;

        protected double _milliSecondMultiplier = 1;
        public double MilliSecondMultiplier => _milliSecondMultiplier;
        
        /// <summary>
        /// Call this before using the timer
        /// </summary>
        public virtual void Init()
        {
            //in order to avoid a switch or general branching calls at runtime, this is done initially!

            switch (this.timerType)
            {
                case TimerType.Seconds:
                    //example:
                    //Total Milli = 5000, MaxTime = 5 (sec)
                    //Expected Calc: MaxTime * 1000 <= TotalMilli -> 5000 <= 5000 -> true

                    _milliSecondMultiplier = 1000;
                    break;

                case TimerType.Minutes:
                    //example:
                    //Total Milli = 5000, MaxTime = 5 (minutes)
                    //Expected Calc: MaxTime * 1000 * 60 < TotalMilli -> 60000 * 5 < 5000 -> false

                    _milliSecondMultiplier = 1000 * 60;
                    break;

                case TimerType.Hours:
                    //example:
                    //Total Milli = 5000, MaxTime = 5 (hours)
                    //Expected Calc: MaxTime * 1000 * 60 * 60 < TotalMilli -> 3600000 * 5 < 5000 -> false

                    _milliSecondMultiplier = 1000 * 60 * 60;

                    break;

                case TimerType.Days:
                    //example:
                    //Total Milli = 5000, MaxTime = 5 (days)
                    //Expected Calc: MaxTime * 1000 * 60 * 60 * 24 < TotalMilli -> 3600000 * 24 * 5 < 5000 -> false

                    _milliSecondMultiplier = 1000 * 60 * 60 * 24;

                    break;

                default:
                    _milliSecondMultiplier = 1;
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="checkAgainst"></param>
        /// <returns>
        /// bool: false = inTime, true = time reached or exceeded
        /// </returns>
        public virtual bool CheckTimeReachedOrExceeded(DateTime checkAgainst)
        {
            //Instant represents no check/ infinite
            if (this.timerType == TimerType.Instant)
                return true;
            if (this.timerType == TimerType.Never)
                return false;

            TimeSpan timeSpan = DateTime.Now - checkAgainst;

            return UnityEngine.Random.Range(this.minMaxRandomTimeRange.x, this.minMaxRandomTimeRange.y) * _milliSecondMultiplier <= timeSpan.TotalMilliseconds;
        }

        public virtual bool CheckTimeReachedOrExceeded()
        {
            return this.CheckTimeReachedOrExceeded(this.CheckAgainstTime);
        }

        public virtual object Clone()
        {
            TimingHelper timing = new TimingHelper();
            timing.minMaxRandomTimeRange = new Vector2(this.minMaxRandomTimeRange.x, this.minMaxRandomTimeRange.y);
            timing._milliSecondMultiplier = _milliSecondMultiplier;
            timing.CheckAgainstTime = new DateTime(this.CheckAgainstTime.Year, this.CheckAgainstTime.Month,
                this.CheckAgainstTime.Day, this.CheckAgainstTime.Hour, this.CheckAgainstTime.Minute, this.CheckAgainstTime.Second, this.CheckAgainstTime.Millisecond);
            timing.timerType = this.timerType;
            
            return timing;
        }
    }
}
