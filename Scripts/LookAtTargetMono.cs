using NaughtyAttributes;
using UnityEngine;

namespace NikosAssets.Helpers
{
    /// <summary>
    /// Looks at the <see cref="target"/> with clamped euler values (if setup)
    /// </summary>
    public class LookAtTargetMono : BaseNotesMono
    {
        /// <summary>
        /// The target to look at
        /// </summary>
        [SerializeField]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        [Tooltip("The target to look at")]
        protected Transform target = default;

        /// <summary>
        /// Ignore this axis angle
        /// </summary>
        [SerializeField]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        [Tooltip("Ignore this axis angle")]
        protected bool keepEulerX, keepEulerY, keepEulerZ;

        /// <summary>
        /// Align the look rotation of this transform to the <see cref="target"/>'s local up <see cref="Vector3"/>
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

        protected virtual void Update()
        {
            Vector3 prevEuler = this.transform.eulerAngles;

            this.transform.rotation = NumericHelper.LookAt(this.target.position, this.transform.position, Vector3.zero);

            if (this.alignWithTargetsUp)
            {
                this.transform.rotation = Quaternion.LookRotation(this.transform.forward, target.up);
            }

            this.transform.eulerAngles = new Vector3(
                this.keepEulerX ? prevEuler.x : this.transform.eulerAngles.x,
                this.keepEulerY ? prevEuler.y : this.transform.eulerAngles.y,
                this.keepEulerZ ? prevEuler.z : this.transform.eulerAngles.z);

            this.transform.eulerAngles += this.eulerOffset;
        }
    }
}
