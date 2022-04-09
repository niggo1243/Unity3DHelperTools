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

        public float CheckAgainstRunningTime { get; set; }
        
        public DateTime CheckAgainstDateTime { get; set; } = DateTime.Now;

        [SerializeField]
        [HideInInspector]
        protected double _secondsMultiplier = 1;
        public double SecondsMultiplier => _secondsMultiplier;
        
        [SerializeField]
        [HideInInspector]
        protected double _milliSecondsMultiplier = 1;
        public double MilliSecondsMultiplier => _milliSecondsMultiplier;
        
        /// <summary>
        /// Call this before using the timer
        /// </summary>
        public virtual void Init()
        {
            //in order to avoid a switch or general branching calls at runtime, this is done initially!

            switch (this.timerType)
            {
                case TimerType.MilliSeconds:
                    _secondsMultiplier = 0.001f;
                    break;
                
                case TimerType.Seconds:
                    _secondsMultiplier = 1;
                    break;

                case TimerType.Minutes:
                    _secondsMultiplier = 60;
                    break;

                case TimerType.Hours:
                    _secondsMultiplier = 60 * 60;
                    break;

                case TimerType.Days:
                    _secondsMultiplier = 60 * 60 * 24;
                    break;

                default:
                    _secondsMultiplier = 1;
                    break;
            }
            
            _milliSecondsMultiplier = _secondsMultiplier * 1000;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="checkAgainst"></param>
        /// <returns>
        /// bool: false = inTime, true = time reached or exceeded
        /// </returns>
        public virtual bool CheckRunningTimeReachedOrExceeded(float checkAgainst)
        {
            //Instant represents no check/ infinite
            if (this.timerType == TimerType.Instant)
                return true;
            if (this.timerType == TimerType.Never)
                return false;
            
            float timeSpan = Time.time - checkAgainst;

            return UnityEngine.Random.Range(this.minMaxRandomTimeRange.x, this.minMaxRandomTimeRange.y) * _secondsMultiplier <= timeSpan;
        }
        
        public virtual bool CheckRunningTimeReachedOrExceeded()
        {
            return this.CheckRunningTimeReachedOrExceeded(this.CheckAgainstRunningTime);
        }

        public virtual bool CheckDateTimeReachedOrExceeded(DateTime checkAgainst)
        {
            //Instant represents no check/ infinite
            if (this.timerType == TimerType.Instant)
                return true;
            if (this.timerType == TimerType.Never)
                return false;
            
            TimeSpan timeSpan = DateTime.Now - checkAgainst;

            return UnityEngine.Random.Range(this.minMaxRandomTimeRange.x, this.minMaxRandomTimeRange.y) * _milliSecondsMultiplier <= timeSpan.TotalMilliseconds;
        }
        
        public virtual bool CheckDateTimeReachedOrExceeded()
        {
            return this.CheckDateTimeReachedOrExceeded(this.CheckAgainstDateTime);
        }

        public virtual object Clone()
        {
            TimingHelper timing = new TimingHelper();
            timing.minMaxRandomTimeRange = new Vector2(this.minMaxRandomTimeRange.x, this.minMaxRandomTimeRange.y);
            timing._secondsMultiplier = _secondsMultiplier;
            timing.CheckAgainstRunningTime = CheckAgainstRunningTime;
            timing.CheckAgainstDateTime = new DateTime(this.CheckAgainstDateTime.Year, this.CheckAgainstDateTime.Month,
                this.CheckAgainstDateTime.Day, this.CheckAgainstDateTime.Hour, this.CheckAgainstDateTime.Minute, this.CheckAgainstDateTime.Second, this.CheckAgainstDateTime.Millisecond);
            timing.timerType = this.timerType;
            
            return timing;
        }
    }
}
