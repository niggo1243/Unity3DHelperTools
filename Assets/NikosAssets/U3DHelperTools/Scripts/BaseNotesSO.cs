using NaughtyAttributes;
using UnityEngine;

namespace NikosAssets.Helpers
{
    /// <summary>
    /// A helper class you can inherit from to add descriptions to your custom <see cref="ScriptableObject"/>s
    /// </summary>
    public abstract class BaseNotesSO : ScriptableObject
    {
#if UNITY_EDITOR
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_EDITORONLY)]
        public string nameNote;

        [ResizableTextArea] [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_EDITORONLY)]
        public string descriptionNote;
#endif
    }
}
