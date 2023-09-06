using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace NikosAssets.Helpers.LookAtTarget
{
    /// <summary>
    /// Looks at the <see cref="_target"/> with clamped euler values (if setup)
    /// </summary>
    public abstract class BaseLookAtTargetMono : BaseNotesMono
    {
        /// <summary>
        /// The target to look at
        /// </summary>
        [FormerlySerializedAs("target")]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        [Tooltip("The target to look at")]
        [SerializeField]
        protected Transform _target = default;

        /// <summary>
        /// Ignore this axis angle
        /// </summary>
        [SerializeField]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        [Tooltip("Ignore this axis angle")]
        protected bool keepEulerX, keepEulerY, keepEulerZ;

        /// <summary>
        /// Align the look rotation of this transform to the <see cref="_target"/>'s local up <see cref="Vector3"/>
        /// </summary>
        [SerializeField]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        [Tooltip("Align the look rotation of this transform to the target's local up vector")]
        protected bool alignWithTargetsUp;

        /// <summary>
        /// Offset the final applied rotation
        /// </summary>
        [SerializeField]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        [Tooltip("Offset the final applied rotation")]
        protected Vector3 eulerOffset = default;

        public Vector3 GetEulerOffset() => eulerOffset;

        public void SetEulerOffset(Vector3 offset)
        {
            eulerOffset = offset;
        }

        public virtual Transform GetTarget() => _target;
        
        public virtual void SetTarget(Transform target)
        {
            _target = target;
        }

        /// <summary>
        /// Returns the <see cref="Quaternion"/> rotation to look at <see cref="_target"/> without applying it
        /// </summary>
        /// <returns><see cref="Quaternion"/></returns>
        public virtual Quaternion GetDesiredLookAtRotation()
        {
            return NumericHelper.LookAt(transform, _target, alignWithTargetsUp);
        }

        /// <summary>
        /// Given the original, yet unrotated "<paramref name="originalEulerAngles"/>"
        /// and the rotated (lerped or final) "<paramref name="currentEulerAngles"/>",
        /// return the eulerAngles to keep from the original (see <see cref="keepEulerX"/>, <see cref="keepEulerY"/>, <see cref="keepEulerZ"/>)
        /// and apply from the new ones.
        /// Finally the <see cref="eulerOffset"/> is added to the result. 
        /// </summary>
        /// <param name="originalEulerAngles"><see cref="Vector3"/></param>
        /// <param name="currentEulerAngles"><see cref="Vector3"/></param>
        /// <returns><see cref="Vector3"/></returns>
        public virtual Vector3 GetFilteredAndAdjustedEulerAngles(Vector3 originalEulerAngles, Vector3 currentEulerAngles)
        {
            return new Vector3
                (
                    this.keepEulerX ? originalEulerAngles.x : currentEulerAngles.x,
                    this.keepEulerY ? originalEulerAngles.y : currentEulerAngles.y,
                    this.keepEulerZ ? originalEulerAngles.z : currentEulerAngles.z
                )
                + this.eulerOffset;
        }
        
        /// <summary>
        /// Executes the look at behaviour
        /// </summary>
        public virtual void Tick()
        {
            Transform tr = this.transform;
            Vector3 prevEuler = tr.eulerAngles;

            tr.rotation = GetDesiredLookAtRotation();

            Vector3 eulerAngles = tr.eulerAngles;
            tr.eulerAngles = GetFilteredAndAdjustedEulerAngles(prevEuler, eulerAngles);
        }
        
#if UNITY_EDITOR

        [Button("Tick Now!")]
        private void Button_TickNow()
        {
            Tick();
        }
        
#endif
    }
}
