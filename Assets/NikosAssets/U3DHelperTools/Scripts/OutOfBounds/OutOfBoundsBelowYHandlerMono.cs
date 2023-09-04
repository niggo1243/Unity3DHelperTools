using NaughtyAttributes;
using NikosAssets.Helpers.AlarmClock;
using UnityEngine;

namespace NikosAssets.Helpers.OutOfBounds
{
    public class OutOfBoundsBelowYHandlerMono : BaseNotesMono
    {
        [SerializeField]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        protected Transform _target;

        [SerializeField]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        protected AlarmClockMono _alarmClockMono;
        
        [SerializeField]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        protected float _outOfBoundsBelowY = -5;

        [SerializeField]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        protected float _yResetPosOffset = 3f;
        
        [SerializeField]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        protected float _yDefaultValueOnNoHit = 7f;

        [SerializeField]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        protected float _shootRayFromGlobalYPos = 100;

        protected virtual void OnEnable()
        {
            _alarmClockMono.OnAlarm += Tick;
        }

        protected virtual void OnDisable()
        {
            _alarmClockMono.OnAlarm -= Tick;
        }

        protected virtual void ResetPos()
        {
            Vector3 tPos = _target.position;
            Vector3 rayOriginPoint = new Vector3(tPos.x, _shootRayFromGlobalYPos, tPos.z);

            if (Physics.Raycast(rayOriginPoint, Vector3.down, out RaycastHit hit))
            {
                _target.position = hit.point + (Vector3.up * _yResetPosOffset);
            }
            else
            {
                _target.position = new Vector3(tPos.x, _yDefaultValueOnNoHit, tPos.z);
            }
        }
        
        protected virtual void Tick()
        {
            if (_target.position.y < _outOfBoundsBelowY)
            {
                ResetPos();    
            }
        }
    }
}
