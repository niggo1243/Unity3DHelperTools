using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace NikosAssets.Helpers
{
    [Serializable]
    public class AlarmUnityEvent : UnityEvent{}
    
    /// <summary>
    /// A helper class that emits <see cref="OnAlarmUnityEvent"/> and <see cref="OnAlarm"/> events
    /// using the <see cref="TimingHelper"/> if the time gets exceeded.
    /// </summary>
    public class AlarmClockMono : BaseNotesMono
    {
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_EVENTS)]
        public AlarmUnityEvent OnAlarmUnityEvent = new AlarmUnityEvent();
        public event Action OnAlarm;
        
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
            timer.CheckAgainstRunningTime = Time.time;
        }

        protected virtual void Update()
        {
            if (timer.CheckRunningTime())
            {
                // invoke the timing events
                OnAlarm?.Invoke();
                OnAlarmUnityEvent?.Invoke();
                
                timer.CheckAgainstRunningTime = Time.time;
            }
        }
    }
}
