using System;

using NaughtyAttributes;
using UnityEngine;

namespace NikosAssets.Helpers
{
    /// <summary>
    /// A helper class to handle in-game (Application) time and overall <see cref="DateTime"/> as well
    /// </summary>
    [Serializable]
    public class TimingHelper : ICloneable
    {
        /// <summary>
        /// The available time frames to check
        /// </summary>
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
        
        /// <summary>
        /// Which time frames do you want to check?
        /// </summary>
        [Tooltip("Which time frames do you want to check?")]
        public TimerType timerType = TimerType.Instant;

        private bool _HideIf_MinMaxTime() => timerType == TimerType.Instant || timerType == TimerType.Never;
        
        /// <summary>
        /// Inclusive times to randomly generate
        /// </summary>
        [HideIf(nameof(_HideIf_MinMaxTime))]
        [AllowNesting]
        [Tooltip("Inclusive times to randomly generate")]
        public Vector2 minMaxRandomTimeRange = new Vector2(10, 10);

        /// <summary>
        /// Update this value, whenever you want to reset the timer to a certain time
        /// </summary>
        public float CheckAgainstRunningTime { get; set; }
        
        /// <summary>
        /// Update this value, whenever you want to reset the timer to a certain time
        /// </summary>
        public DateTime CheckAgainstDateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// Used in <see cref="Time"/>.<see cref="Time.time"/> checks
        /// </summary>
        [SerializeField]
        [HideInInspector]
        protected double _secondsMultiplier = 1;
        /// <summary>
        /// Used in <see cref="Time"/>.<see cref="Time.time"/> checks
        /// </summary>
        public double SecondsMultiplier => _secondsMultiplier;
        
        /// <summary>
        /// Used in <see cref="DateTime"/> checks
        /// </summary>
        [SerializeField]
        [HideInInspector]
        protected double _milliSecondsMultiplier = 1;
        
        /// <summary>
        /// Used in <see cref="DateTime"/> checks
        /// </summary>
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
        
        public virtual void ResetTimers()
        {
            ResetRunningTime();
            ResetDateTime();
        }

        public virtual void ResetRunningTime()
        {
            CheckAgainstRunningTime = Time.time;
        }

        public virtual void ResetDateTime()
        {
            CheckAgainstDateTime = DateTime.Now;
        }

        /// <summary>
        /// Checks if the in-game time is reached using the "<paramref name="checkAgainst"/>" time
        /// </summary>
        /// <param name="checkAgainst"></param>
        /// <returns>
        /// bool: false = inTime, true = time reached or exceeded
        /// </returns>
        public virtual bool CheckRunningTime(float checkAgainst)
        {
            //Instant represents no check/ infinite
            if (this.timerType == TimerType.Instant)
                return true;
            if (this.timerType == TimerType.Never)
                return false;
            
            float timeSpan = Time.time - checkAgainst;

            return UnityEngine.Random.Range(this.minMaxRandomTimeRange.x, this.minMaxRandomTimeRange.y) * _secondsMultiplier <= timeSpan;
        }
        
        /// <summary>
        /// Checks if the in-game time is reached using the local <see cref="CheckAgainstRunningTime"/>
        /// </summary>
        /// <returns>
        /// bool: false = inTime, true = time reached or exceeded
        /// </returns>
        public virtual bool CheckRunningTime()
        {
            return this.CheckRunningTime(this.CheckAgainstRunningTime);
        }

        /// <summary>
        /// Checks if the <see cref="DateTime"/> is reached using the "<paramref name="checkAgainst"/>" time
        /// </summary>
        /// <param name="checkAgainst"></param>
        /// <returns>
        /// bool: false = inTime, true = time reached or exceeded
        /// </returns>
        public virtual bool CheckDateTime(DateTime checkAgainst)
        {
            //Instant represents no check/ infinite
            if (this.timerType == TimerType.Instant)
                return true;
            if (this.timerType == TimerType.Never)
                return false;
            
            TimeSpan timeSpan = DateTime.Now - checkAgainst;

            return UnityEngine.Random.Range(this.minMaxRandomTimeRange.x, this.minMaxRandomTimeRange.y) * _milliSecondsMultiplier <= timeSpan.TotalMilliseconds;
        }
        
        /// <summary>
        /// Checks if the <see cref="DateTime"/> is reached using the local <see cref="CheckAgainstDateTime"/>
        /// </summary>
        /// <returns>
        /// bool: false = inTime, true = time reached or exceeded
        /// </returns>
        public virtual bool CheckDateTime()
        {
            return this.CheckDateTime(this.CheckAgainstDateTime);
        }

        /// <summary>
        /// Fast cloning
        /// </summary>
        /// <returns>
        /// An independent clone of this object
        /// </returns>
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
