using System;

namespace NikosAssets.Helpers.AlarmClock
{
    public class AlarmClockDateMono : BaseAlarmClock
    {
        public override bool CheckTime() => timer.CheckDateTime();

        public override void ResetTime()
        {
            timer.CheckAgainstDateTime = DateTime.Now;
        }
    }
}
