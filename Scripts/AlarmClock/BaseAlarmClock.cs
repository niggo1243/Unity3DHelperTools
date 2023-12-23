using System;
using NaughtyAttributes;
using UnityEngine;

namespace NikosAssets.Helpers.AlarmClock
{
    /// <summary>
    /// Base class for stopwatch like behaviour handling with C# event and UnityEvent support
    /// </summary>
    public abstract class BaseAlarmClock : BaseNotesMono
    {
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_EVENTS)]
        public AlarmUnityEvent OnAlarmUnityEvent = new AlarmUnityEvent();
        public virtual event Action OnAlarm;
        
        /// <summary>
        /// The <see cref="TimingHelper"/> to check when the <see cref="OnAlarmUnityEvent"/> and <see cref="OnAlarm"/> events should be emitted
        /// </summary>
        [Tooltip("The TimingHelper to check when the OnAlarmUnityEvent and OnAlarm events should be emitted")]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        public TimingHelper timer = new TimingHelper(TimingHelper.TimerType.Seconds, Vector2.one);

        protected virtual void OnValidate()
        {
            timer.Init();
        }

        protected virtual void Start()
        {
            timer.Init();
            ResetTime();
        }

        protected virtual void Update()
        {
            Tick();
        }

        /// <summary>
        /// Call this to check if the set time of <see cref="timer"/> is up and emit events (<see cref="OnAlarm"/> and <see cref="OnAlarmUnityEvent"/>) if that is the case
        /// </summary>
        public virtual void Tick()
        {
            if (CheckTime())
            {
                // invoke the timing events
                OnAlarm?.Invoke();
                OnAlarmUnityEvent?.Invoke();
                
                ResetTime();
            }
        }
        
        public abstract bool CheckTime();

        public abstract void ResetTime();
    }
}
