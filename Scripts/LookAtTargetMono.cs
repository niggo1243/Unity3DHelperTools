using UnityEngine;

namespace NikosAssets.Helpers
{
    public class LookAtTargetMono : BaseNotesMono
    {
        public Transform target = default;

        [SerializeField]
        private bool keepEulerX = default, keepEulerY = default, keepEulerZ = default;

        [SerializeField]
        private bool alignWithTargetsUp = default;

        [SerializeField]
        private Vector3 eulerOffset = default;

        // Update is called once per frame
        private void Update()
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
