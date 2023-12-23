using System;

namespace NikosAssets.Helpers.AlarmClock
{
    /// <summary>
    /// A helper class that emits <see cref="OnAlarmUnityEvent"/> and <see cref="OnAlarm"/> events
    /// using the <see cref="TimingHelper"/> if the time gets exceeded using <see cref="DateTime"/>
    /// </summary>
    public class AlarmClockDateMono : BaseAlarmClock
    {
        public override bool CheckTime() => timer.CheckDateTime();

        public override void ResetTime()
        {
            timer.CheckAgainstDateTime = DateTime.Now;
        }
    }
}
