using System;
using UnityEngine;
using UnityEngine.Events;

namespace NikosAssets.Helpers
{
    [Serializable]
    public class AlarmUnityEvent : UnityEvent{}
    
    public class AlarmClockMono : MonoBehaviour
    {
        public AlarmUnityEvent OnAlarmUnityEvent = new AlarmUnityEvent();
        public event Action OnAlarm;
        
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
