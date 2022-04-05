using NaughtyAttributes;
using UnityEngine;

namespace NikosAssets.Helpers
{
    public class LookAtTargetMono : BaseNotesMono
    {
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        public Transform target = default;

        [SerializeField]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        protected bool keepEulerX = default, keepEulerY = default, keepEulerZ = default;

        [SerializeField]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        protected bool alignWithTargetsUp = default;

        [SerializeField]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        protected Vector3 eulerOffset = default;

        // Update is called once per frame
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
