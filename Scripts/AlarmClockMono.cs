using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace NikosAssets.Helpers
{
    [Serializable]
    public class AlarmUnityEvent : UnityEvent{}
    
    public class AlarmClockMono : BaseNotesMono
    {
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_EVENTS)]
        public AlarmUnityEvent OnAlarmUnityEvent = new AlarmUnityEvent();
        public event Action OnAlarm;
        
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_DESCRIPTIONS)]
        public TimingHelper timer = new TimingHelper(TimingHelper.TimerType.Seconds, Vector2.one);

        protected virtual void OnValidate()
        {
            timer.Init();
        }

        protected virtual void Start()
        {
            timer.Init();
            timer.CheckAgainstTime = DateTime.Now;
        }

        protected virtual void Update()
        {
            if (timer.CheckTimeReachedOrExceeded())
            {
                // invoke the timing events
                OnAlarm?.Invoke();
                OnAlarmUnityEvent?.Invoke();
                
                timer.CheckAgainstTime = DateTime.Now;
            }
        }
    }
}
