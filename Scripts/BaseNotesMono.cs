using UnityEngine;
using NaughtyAttributes;

namespace NikosAssets.Helpers
{
    public abstract class BaseNotesMono : MonoBehaviour
    {
#if UNITY_EDITOR
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_EDITORONLY)]
        public string nameNote;
        [ResizableTextArea]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_EDITORONLY)]
        public string descriptionNote;
#endif
    }
}
