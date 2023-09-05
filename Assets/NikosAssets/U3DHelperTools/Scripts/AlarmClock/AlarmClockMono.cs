namespace NikosAssets.Helpers.AlarmClock
{
    /// <summary>
    /// A helper class that emits <see cref="OnAlarmUnityEvent"/> and <see cref="OnAlarm"/> events
    /// using the <see cref="TimingHelper"/> if the time gets exceeded using <see cref="UnityEngine.Time"/>.<see cref="UnityEngine.Time.time"/>
    /// </summary>
    public class AlarmClockMono : BaseAlarmClock
    {
        public override bool CheckTime() => timer.CheckRunningTime();
        
        public override void ResetTime()
        {
            timer.ResetRunningTime();
        }
    }
}
