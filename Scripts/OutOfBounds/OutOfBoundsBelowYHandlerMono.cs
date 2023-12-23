using NaughtyAttributes;
using NikosAssets.Helpers.AlarmClock;
using NikosAssets.Helpers.Extensions;
using UnityEngine;

namespace NikosAssets.Helpers.OutOfBounds
{
    /// <summary>
    /// A very simple helper script for relatively flat environments, where if the <see cref="_target"/> falls below a certain
    /// threshold (<see cref="_outOfBoundsBelowY"/>), the <see cref="_target"/> is replaced again 
    /// </summary>
    public class OutOfBoundsBelowYHandlerMono : BaseNotesMono
    {
        /// <summary>
        /// The target to observe and reset if lost below <see cref="_outOfBoundsBelowY"/>
        /// </summary>
        [SerializeField]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        [Tooltip("The target to observe and reset if lost below " + nameof(_outOfBoundsBelowY))]
        protected Transform _target;

        /// <summary>
        /// The interval to check if the <see cref="_target"/> is out of bounds
        /// </summary>
        [SerializeField]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        [Tooltip("The interval to check if the " + nameof(_target) + " is out of bounds")]
        protected AlarmClockMono _alarmClockMono;
        
        /// <summary>
        /// Reset if <see cref="_target"/> is below this threshold
        /// </summary>
        [SerializeField]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        [Tooltip("Reset if " + nameof(_target) + " is below this threshold")]
        protected float _outOfBoundsBelowY = -5;

        /// <summary>
        /// When resetting the <see cref="_target"/> on the surface, this is the y offset
        /// </summary>
        [SerializeField]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        [Tooltip("When resetting the " + nameof(_target) + " on the surface, this is the y offset")]
        protected float _yResetPosOffset = 3f;
        
        /// <summary>
        /// When no surface was found by the raycast, respawn the <see cref="_target"/> at this height
        /// </summary>
        [SerializeField]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        [Tooltip("When no surface was found by the raycast, respawn the " + nameof(_target) + " at this height")]
        protected float _yDefaultValueOnNoHit = 7f;

        /// <summary>
        /// The origin height to shoot the surface checker raycast from
        /// </summary>
        [SerializeField]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        [Tooltip("The origin height to shoot the surface checker raycast from")]
        protected float _shootRayFromGlobalYPos = 100;

        protected virtual void OnEnable()
        {
            _alarmClockMono.OnAlarm += Tick;
        }

        protected virtual void OnDisable()
        {
            _alarmClockMono.OnAlarm -= Tick;
        }

        /// <summary>
        /// Resets the <see cref="_target"/> by shooting a ray from top to bottom and placing the <see cref="_target"/>
        /// on top of the hit position with the <see cref="_yResetPosOffset"/>.
        /// If no surface was found, reset the <see cref="_target"/> at the <see cref="_yDefaultValueOnNoHit"/> height.
        /// </summary>
        public virtual void ResetPos()
        {
            Vector3 tPos = _target.position;
            Vector3 rayOriginPoint = tPos.GetWithNewY(_shootRayFromGlobalYPos);

            if (Physics.Raycast(rayOriginPoint, Vector3.down, out RaycastHit hit))
            {
                _target.position = hit.point + (Vector3.up * _yResetPosOffset);
            }
            else
            {
                _target.position = tPos.GetWithNewY(_yDefaultValueOnNoHit);
            }
        }
        
        /// <summary>
        /// Executes the out of bounds check
        /// </summary>
        public virtual void Tick()
        {
            if (_target.position.y < _outOfBoundsBelowY)
            {
                ResetPos();    
            }
        }
    }
}
